using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Domain.Enums
{
    public enum Policy
    {
        GetRoles,
        ManageRoles,
        ManagePolicies,
        ManageEntities,
        ManageUsers,
        GetUsers
    }
    public enum PolicyType
    {
        Permission
    }
}
