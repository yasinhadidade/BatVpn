using BatVpn.Application.Handler.VpnConfig;
using BatVpn.Domain.Entities;
using BatVpn.Infrastructure.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Application.Command.VpnConfig
{
    public class SendOpenVpnConfigCommand:IRequest<OperationResult<dockertest>>
    {
        public string Email { get; set; }

        public SendOpenVpnConfigCommand(string email)
        {
            Email = email;
        }
    }
}
