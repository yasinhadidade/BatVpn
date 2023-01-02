using Batvpn.Persistence.Repository.OpenVpnConfigRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batvpn.Persistence
{
    public class PersistenceBootstrapper
    {
        public static void Configure(IServiceCollection services, string BatVpnconnectionString,string IdentityConnectionString)
        {

            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(IdentityConnectionString,b => b.MigrationsAssembly("BatVpn.Api")).EnableSensitiveDataLogging());
            services.AddDbContext<BatVpnDbContext>(option => option.UseSqlServer(BatVpnconnectionString,b=>b.MigrationsAssembly("BatVpn.Api")).EnableSensitiveDataLogging());

            services.AddScoped<IOpenVpnConfigRepository,OpenVpnConfigRepository>();

        }
    }
}
