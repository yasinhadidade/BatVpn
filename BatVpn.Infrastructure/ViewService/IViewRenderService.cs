using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure.ViewService
{
    public interface IViewRenderService
    {
        string RenderToStringAsync(string viewName, object model);
    }
}
