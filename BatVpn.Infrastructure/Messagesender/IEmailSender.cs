using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatVpn.Infrastructure.Messagesender
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string body);
    }
}
