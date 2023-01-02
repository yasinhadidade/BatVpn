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
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using V2Ray.Core.App.Proxyman.Command;
using V2Ray.Core.Proxy.Vmess.Inbound;
using V2Ray.Core.App.Proxyman;

namespace BatVpn.Application.Handler.VpnConfig
{
    public class SendOpenVpnConfigCommandHandler : IRequestHandler<SendOpenVpnConfigCommand, OperationResult<dockertest>>
    {
        private readonly IEmailSender _emailSender;
        private readonly IOpenVpnConfigRepository _openVpnConfigRepository;
        private readonly IViewRenderService _viewRenderService;
        private OperationResult<dockertest> operation;

        public SendOpenVpnConfigCommandHandler(IEmailSender emailSender,
            IOpenVpnConfigRepository openVpnConfigRepository,
            IViewRenderService viewRenderService)
        {
            _emailSender = emailSender;
            operation = new OperationResult<dockertest>();
            _openVpnConfigRepository = openVpnConfigRepository;
            _viewRenderService = viewRenderService;
        }

        public async Task<OperationResult<dockertest>> Handle(SendOpenVpnConfigCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
                return operation.Failed(ApplicationMessage.InvalidUser);

            var config = new OpenVpnConfig()
            {
                CreationDate = DateTime.Now,
                Direction = "root",
                ExpireDate = DateTime.Now.AddDays(10),
                UniqueName = request.Email
            };
            var entity = await _openVpnConfigRepository.AddAsync(config);

            return operation.Succedded(new dockertest()
            {
                name = entity.UniqueName,
                family = entity.Direction
            });
            //var entity = new dockertest()
            //{
            //    name = request.Email,
            //    family = "my family"
            //};
            //return operation.Succedded(entity);




        }
    }


    public class dockertest
    {
        public string name { get; set; }
        public string family { get; set; }
    }
}
