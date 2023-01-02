using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure.AuthUtility
{
    public interface IAuthHelper
    {
        AuthViewModel CurrentAccountInfo();
    }
}
