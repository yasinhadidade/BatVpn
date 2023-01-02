using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BatVpn.Infrastructure.AuthUtility
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public AuthViewModel CurrentAccountInfo()
        {
            var claims = _contextAccessor.HttpContext.User.Claims.ToList();
            var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return new AuthViewModel { UserId = userId };
        }
    }
}
