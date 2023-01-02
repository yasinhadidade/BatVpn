using BatVpn.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure.Authorization
{
    public class Authorize : Attribute, IAuthorizationFilter
    {
        private readonly IEnumerable<Policy> policies = new List<Policy>();
        private readonly IEnumerable<Role> roles = new List<Role>();

        public Authorize(params Policy[] policies)
        {
            this.policies = policies;
        }
        public Authorize(params Role[] roles)
        {
            this.roles = roles;
        }
        public Authorize()
        {
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {

                context.Result = new UnauthorizedResult();
                return;
            }

            if (policies.Any() || roles.Any())
            {
                var hasClaim = user.Claims.Any(c => c.Type == ClaimTypes.Role && roles.Any(a => Enum.GetName(a) == c.Value) ||
                c.Type == Enum.GetName(PolicyType.Permission) && policies.Any(a => Enum.GetName(a) == c.Value));
                if (!hasClaim)
                {
                    context.Result = new ForbidResult();

                }
            }
        }


    }
}
