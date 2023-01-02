using Batvpn.Persistence.Common;
using BatVpn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batvpn.Persistence.Repository.OpenVpnConfigRepository
{
    public class OpenVpnConfigRepository : BaseRepository<OpenVpnConfig>, IOpenVpnConfigRepository
    {
        private readonly BatVpnDbContext _context;
        public OpenVpnConfigRepository(BatVpnDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
