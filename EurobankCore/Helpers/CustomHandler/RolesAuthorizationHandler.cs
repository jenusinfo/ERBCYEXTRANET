using CMS.Membership;
using Eurobank.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eurobank.Helpers.CustomHandler
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RolesAuthorizationRequirement requirement)
        {
            if(context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validRole = false;
            if(requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userName = claims.FirstOrDefault();
                //var userName1 = claims.FirstOrDefault(c => c.Type == "name").Value;
                var roles = requirement.AllowedRoles;
                var claimTypes = claims.FirstOrDefault(
             c => c.Type == ClaimTypes.Role)?.Value;
                if(claimTypes != null)
                {
                    foreach(var item in requirement.AllowedRoles)
                    {
                        if(item == claimTypes)
                        {
                            validRole = true;

                        }

                    }

                }
                if(userName.Value.ToUpper() == "ADMINISTRATOR")
                {
                    validRole = true;
                }
				
                // validRole = new UserModel().GetUsers().Where(p => roles.Contains(p.Role) && p.UserName == userName).Any();
            }

            if(validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
