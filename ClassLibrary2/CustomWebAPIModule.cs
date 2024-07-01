
using System.Web.Http;

using CMS;
using CMS.DataEngine;

// Registers the custom module into the system
[assembly: RegisterModule(typeof(ClassLibrary2.CustomWebAPIModule))]

namespace ClassLibrary2
{
    public class CustomWebAPIModule : Module
    {
        // Module class constructor, the system registers the module under the name "CustomWebAPI"
        public CustomWebAPIModule()
        : base("CustomWebAPI")
        {
        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();

            // Registers a "customapi" route
            GlobalConfiguration.Configuration.Routes.MapHttpRoute("customapi", "customapi/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}
