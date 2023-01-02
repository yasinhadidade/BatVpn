using BatVpn.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace BatVpn.Domain.Entities
{
    public class OpenVpnConfig:BaseEntity
    {
        public string Direction { get; set; }
        public string UniqueName { get; set; }
        public DateTime ExpireDate { get; set; }

    }
}
