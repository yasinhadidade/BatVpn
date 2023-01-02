using BatVpn.Infrastructure.AuthUtility;
using BatVpn.Infrastructure.Http;
using BatVpn.Infrastructure.Messagesender;
using BatVpn.Infrastructure.Redis;
using BatVpn.Infrastructure.ViewService;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure
{
    public class InfrastructureBootstrapper
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IAuthHelper, AuthHelper>();
            services.AddScoped<IRedisClient, RedisClient>();
            services.AddScoped<IHttpClient, BatVpn.Infrastructure.Http.HttpClient>();

            services.AddScoped<IEmailSender,EmailSender>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            //services.AddScoped<IRazorViewEngine, RazorViewEngine>();
        }
    }
}
