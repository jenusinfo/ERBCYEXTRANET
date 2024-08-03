using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

using CMS.Helpers;

using Eurobank.Helpers;
using Eurobank.PageTemplates;

using Kentico.Activities.Web.Mvc;
using Kentico.CampaignLogging.Web.Mvc;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Forms.Web.Mvc;
using Kentico.Membership;
using Kentico.Newsletters.Web.Mvc;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Scheduler.Web.Mvc;
using Kentico.Web.Mvc;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Eurobank.Models.Application.Common;
using Eurobank.Helpers.CustomHandler;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection;
using System.IO;
using CMS.DataEngine;
using CMS.SiteProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Eurobank

{
    public class Startup
    {
        /// <summary>
        /// This is a route controller constraint for pages not handled by the content tree-based router.
        /// The constraint limits the match to a list of specified controllers for pages not handled by the content tree-based router.
        /// The constraint ensures that broken URLs lead to a "404 page not found" page and are not handled by a controller dedicated to the component or 
        /// to a page handled by the content tree-based router (which would lead to an exception).
        /// </summary>
        private const string CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS = "Account|Consent|Coupon|Checkout|NewsletterSubscriptionWidget|Orders|Search|Subscription";

        // Application authentication cookie name
        private const string AUTHENTICATION_COOKIE_NAME = "identity.authentication";

        public const string DEFAULT_WITHOUT_LANGUAGE_PREFIX_ROUTE_NAME = "DefaultWithoutLanguagePrefix";


        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private static int SessionTimeoutMin = SettingsKeyInfoProvider.GetIntValue(SiteContext.CurrentSiteName + ".Session_Timeout_key");

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            // Add framework services.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(config =>
                 {
                     config.Cookie.HttpOnly = true;
                     config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                     config.Cookie.SameSite = SameSiteMode.Strict;
                     //config.Cookie.Name = "UserLoginCookie"; // Name of cookie     
                     config.LoginPath = "/Account/Login"; // Path for the redirect to user login page    
                     config.AccessDeniedPath = "/Account/UserAccessDenied";
                 });


            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            services
                .AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Add Kendo UI services to the services container
            services.AddKendo();



            // Ensures redirect to the administration instance based on URL defined in settings
            services.AddSingleton<IStartupFilter>(new AdminRedirectStartupFilter(Configuration));
            services.AddKendo();

            // Ensures smart search index rebuild upon installation or deployment
            services.AddSingleton<IStartupFilter>(new SmartSearchIndexRebuildStartupFilter());

            var kenticoServiceCollection = services.AddKentico(features =>
            {
                features.UsePageBuilder(new PageBuilderOptions
                {
                    DefaultSectionIdentifier = ComponentIdentifiers.SINGLE_COLUMN_SECTION,
                    RegisterDefaultSection = false
                });
                features.UseActivityTracking();
                features.UseABTesting();
                features.UseWebAnalytics();
                features.UseEmailTracking();
                features.UseCampaignLogger();
                features.UseScheduler();
                features.UsePageRouting(new PageRoutingOptions { EnableAlternativeUrls = true, CultureCodeRouteValuesKey = "culture" });
            })
                .SetAdminCookiesSameSiteNone();

            if (Environment.IsDevelopment())
            {
                kenticoServiceCollection.DisableVirtualContextSecurityForLocalhost();
            }

            services.AddEurobankServices();



            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddLocalization()
                    .AddControllersWithViews()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization(options =>
                    {
                        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
                    });
            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //File.Create(xmlPath);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v1", Title = "Swagger API", Description = "Application Status Change API" });

                //c.IncludeXmlComments(xmlPath);
            });
            services.Configure<KenticoRequestLocalizationOptions>(options =>
            {
                options.RequestCultureProviders.Add(new RouteDataRequestCultureProvider
                {
                    RouteDataStringKey = "culture",
                    UIRouteDataStringKey = "culture"
                });
            });



            services.Configure<FormBuilderBundlesOptions>(options =>
            {
                options.JQueryCustomBundleWebRootPath = "Scripts/jquery-3.7.1.min.js";
                options.JQueryUnobtrusiveAjaxCustomBundleWebRootPath = "Scripts/jquery.unobtrusive-ajax.min.js";
            });

            ConfigureMembershipServices(services);
            ConfigurePageBuilderFilters();

            services.AddHttpContextAccessor();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IAntiforgery antiforgery)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(next => context =>
            {
                if (
                    string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Path.Value, "/index.cshtml", StringComparison.OrdinalIgnoreCase))
                {
                    // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
                    //context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                    //context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    //context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                    //context.Response.Headers.Add("Content-Security-Policy", "'self' 'unsafe-inline' 'unsafe-eval';");

                }

                return next(context);
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
            });
            app.Use(async (context, next) =>
            {
                if (!context.Response.Headers.ContainsKey("X-XSS-Protection"))
                {
                    // Add X-XSS-Protection header
                    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                }

                if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    // Add X-Content-Type-Options header
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    // Add X-Frame-Options header
                    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                }

                if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    // Add Content-Security-Policy header
                    context.Response.Headers.Add("Content-Security-Policy", "font-src 'self' data:;");
                    //context.Response.Headers.Add("Content-Security-Policy", "'self' 'sha256-pILX+5FGCpLRHvNBgtABIdSMmytrYudGxJBUYXY1t0s=' 'unsafe-inline' 'unsafe-eval';");
                }

                // Call the next middleware in the pipeline
                await next.Invoke();
            });
            app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.SameAsRequest
            });
            app.UseSwagger(c => { c.SerializeAsV2 = true; });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/eurobank/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint("/eurobank/swagger/v2/swagger.json", "My API V2");
            });
            ////Added by SUBRAT---------------START
            //app.UseXXssProtection(options => options.EnabledWithBlockMode());
            //app.UseXfo(options => options.Deny());
            //app.UseXContentTypeOptions();
            //app.UseReferrerPolicy(options => options.NoReferrer());
            ////--------------------END
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();

            app.UseKentico();

            //app.UseHttpsRedirection();

            app.UseCookiePolicy();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseKenticoRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Kentico().MapRoutes();

                endpoints.MapControllerRoute(
                   name: "error",
                   pattern: "error/{code}",
                   defaults: new { controller = "HttpErrors", action = "Error" }
                );

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: $"{{culture}}/{{controller}}/{{action}}",
                   constraints: new
                   {
                       culture = new SiteCultureConstraint { HideLanguagePrefixForDefaultCulture = true },
                       controller = CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS
                   }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: DEFAULT_WITHOUT_LANGUAGE_PREFIX_ROUTE_NAME,
                    pattern: "{controller}/{action}",
                    constraints: new
                    {
                        controller = CONSTRAINT_FOR_NON_ROUTER_PAGE_CONTROLLERS
                    }
                );
            });
            //app.Use(next => context =>
            //{
            //	var tokens = antiforgery.GetAndStoreTokens(context);
            //	context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //		new CookieOptions() { HttpOnly = false });
            //	//string path = context.Request.Path.Value;

            //	//if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
            //	//	string.Equals(path, "/applications/", StringComparison.OrdinalIgnoreCase))
            //	//{
            //	//	var tokens = antiforgery.GetAndStoreTokens(context);
            //	//	context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //	//		new CookieOptions() { HttpOnly = false });
            //	//}

            //	return next(context);
            //});

        }


        private static void ConfigureMembershipServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.IdleTimeout = TimeSpan.FromMinutes(SessionTimeoutMin == 0 ? 20 : SessionTimeoutMin);
            });
            services.AddScoped<IPasswordHasher<ApplicationUser>, Kentico.Membership.PasswordHasher<ApplicationUser>>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Note: These settings are effective only when password policies are turned off in the administration settings.
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
                    .AddApplicationDefaultTokenProviders()
                    .AddUserStore<ApplicationUserStore<ApplicationUser>>()
                    .AddRoleStore<ApplicationRoleStore<ApplicationRole>>()
                    .AddUserManager<ApplicationUserManager<ApplicationUser>>()
                    .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddAuthorization();


            services.ConfigureApplicationCookie(c =>
            {
                c.Events.OnRedirectToLogin = ctx =>
                {
                    // Redirects to login page respecting the current culture
                    var factory = ctx.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
                    var urlHelper = factory.GetUrlHelper(new ActionContext(ctx.HttpContext, new RouteData(ctx.HttpContext.Request.RouteValues), new ActionDescriptor()));
                    var url = urlHelper.Action("Login", "Account") + new Uri(ctx.RedirectUri).Query;

                    ctx.Response.Redirect(url);

                    return Task.CompletedTask;
                };
                //c.ExpireTimeSpan = TimeSpan.FromDays(14);
                c.Cookie.HttpOnly = true;
                c.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                c.Cookie.SameSite = SameSiteMode.Strict;
                c.ExpireTimeSpan = TimeSpan.FromMinutes(SessionTimeoutMin == 0 ? 20 : SessionTimeoutMin);
                c.SlidingExpiration = true;
                c.Cookie.Name = AUTHENTICATION_COOKIE_NAME;
            });

            CookieHelper.RegisterCookie(AUTHENTICATION_COOKIE_NAME, CookieLevel.Essential);
        }


        private static void ConfigurePageBuilderFilters()
        {
            PageBuilderFilters.PageTemplates.Add(new ArticlePageTemplatesFilter());
            PageBuilderFilters.PageTemplates.Add(new LandingPageTemplatesFilter());
        }
    }
}
