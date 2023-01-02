using BatVpn.Application.Command.VpnConfig;
using BatVpn.Infrastructure.Constants;
using BatVpn.Infrastructure.Messagesender;
using BatVpn.Infrastructure.Response;
using BatVpn.Infrastructure.RunTimeInfo;
using MediatR;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Batvpn.Persistence.Repository.OpenVpnConfigRepository;
using BatVpn.Domain.Entities;
using BatVpn.Infrastructure.ViewService;
using Microsoft.AspNetCore.Hosting;

namespace BatVpn.Application.Handler.VpnConfig
{
    public class SendOpenVpnConfigCommandHandler : IRequestHandler<SendOpenVpnConfigCommand, OperationResult>
    {
        private readonly IEmailSender _emailSender;
        private readonly IOpenVpnConfigRepository _openVpnConfigRepository;
        private readonly IViewRenderService _viewRenderService;
        private OperationResult operation;

        public SendOpenVpnConfigCommandHandler(IEmailSender emailSender,
            IOpenVpnConfigRepository openVpnConfigRepository,
            IViewRenderService viewRenderService)
        {
            _emailSender = emailSender;
            operation = new OperationResult();
            _openVpnConfigRepository = openVpnConfigRepository;
            _viewRenderService = viewRenderService;
        }

        public async Task<OperationResult> Handle(SendOpenVpnConfigCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
                return operation.Failed(ApplicationMessage.InvalidUser);

            var configName = request.Email.Split(".").First();
            var projectPath = Directory.GetCurrentDirectory() + "\\wwwroot";

            var directoryPath = $"{projectPath}\\OpenVpnConfig";
            var fileName = configName + ".ovpn";
            var filePath = $"{directoryPath}\\{fileName}";
            


            if (OperatingSystemType.IsWindows())
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = false;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                await cmd.StandardInput.WriteLineAsync("start chrome");
                //await cmd.StandardInput.WriteLineAsync("ssh root@5.161.135.134");
                //await cmd.StandardInput.WriteLineAsync("bash openvpn-ubuntu-install.sh");
                //await cmd.StandardInput.WriteLineAsync("1");
                //await cmd.StandardInput.WriteLineAsync(configName);
                //await cmd.StandardInput.WriteLineAsync("exit");
                //string dastor = $"scp root@5.161.135.134:/root{configName}.ovpn {directoryPath}";
                //await cmd.StandardInput.WriteLineAsync(dastor);
                //cmd.StandardInput.Flush();
                //cmd.StandardInput.Close();
                //cmd.WaitForExit();

                if (File.Exists(filePath))
                {
                    var newConfig = new OpenVpnConfig()
                    {
                        Direction = filePath,
                        UniqueName = Guid.NewGuid().ToString(),
                        ExpireDate = DateTime.Now.AddDays(30)
                    };
                    await _openVpnConfigRepository.AddAsync(newConfig);
                    string emailTemp = _viewRenderService.RenderToStringAsync("_UserOpenVpnConfig", newConfig);
                    _emailSender.Send(request.Email, "Your Vpn Config Is Here", emailTemp);
                }
                else
                {

                }


                return operation.Succedded();
            }

            else
            {
                return operation.Failed("this is linux");
            }






            //using (var client = new SshClient("hostnameOrIp", "username", "password"))
            //{
            //    // with command here we select as root, then command "bash openvpn-obunto-install.sh" then write clientName ,
            //    // then download the file in our project . then we will send it to their email
            //    client.Connect();
            //    client.RunCommand("...");
            //    client.Disconnect();
            //}

        }
    }
}
