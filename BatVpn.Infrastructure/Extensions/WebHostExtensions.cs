using Batvpn.Persistence;
using Batvpn.Persistence.Repository.SeedData;
using BatVpn.Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable


namespace BatVpn.Infrastructure.Extensions
{
    //public static class WebHostExtensions
    //{
    //    public static IHost SeedData(this IHost host)
    //    {
    //        using (var scope = host.Services.CreateScope())
    //        {
    //            var services = scope.ServiceProvider;
    //            var context = services.GetService<BatVpnDbContext>();
    //            context.Database.Migrate();
    //            var userManager = services.GetService<UserManager<ApplicationUser>>();
    //            var roleManager = services.GetService<RoleManager<IdentityRole>>();
    //            var seedDB = new DBSeeder(context, userManager, roleManager);
    //            seedDB.SeedData().Wait();
    //        }

    //        return host;
    //    }
    //}

    public static class EnsureMigration
    {
        public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            var context = app.ApplicationServices.GetService<T>();
            context.Database.Migrate();
        }
    }


}
