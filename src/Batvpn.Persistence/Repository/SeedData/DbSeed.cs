using BatVpn.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batvpn.Persistence.Repository.SeedData
{
    public class DBSeeder
    {
        private readonly BatVpnDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public DBSeeder(BatVpnDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task SeedData()
        {
            //await FillStep();
            //await FillSMSSteps();


        }

        //async Task FillSMSSteps()
        //{
        //    if (!context.Steps.Any())
        //    {
        //        var step = await context.Steps.AddAsync(new Step() { Name = "Login" });
        //        await context.SaveChangesAsync();
        //        await context.NotificationTexts.AddAsync(new NotificationText() { Text = "کاربر گرامی  بیوتی ارت، برای ورورد به اپلیکیشن کد زیر را وارد نمایید.\n#code#", StepId = step.Entity.Id });
        //        await context.NotificationTexts.AddAsync(new NotificationText() { Text = "کاربر گرامي براي ورود به  بیوتی ارت کد راواردکنيد.#code#", StepId = step.Entity.Id });
        //        await context.SaveChangesAsync();
        //    }
        //}

        //async Task FillStep()
        //{
        //    if (!context.Steps.Any())
        //    {
        //        await context.Steps.AddAsync(new Step() { Name = "Login" });
        //        await context.SaveChangesAsync();
        //    }
        //}




    }

}
