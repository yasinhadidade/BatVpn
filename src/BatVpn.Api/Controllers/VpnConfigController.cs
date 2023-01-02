using BatVpn.Application.Command.VpnConfig;
using BatVpn.Application.DTOs.VpnConfig;
using BatVpn.Application.Handler.VpnConfig;
using BatVpn.Domain.Entities;
using BatVpn.Infrastructure.Authorization;
using BatVpn.Infrastructure.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BatVpn.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VpnConfigController : ControllerBase
    {
        private readonly IMediator mediator;

        public VpnConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(OperationResult<dockertest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OperationResult<dockertest>>> SendOpenVpnConfig(SendVpnConfigDTO request)
        {
            var command = new SendOpenVpnConfigCommand(request.Email);
            var result = await mediator.Send(command);
            return result;
           
        }



    }


}
