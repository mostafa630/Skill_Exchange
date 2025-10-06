using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [Route("[controller]")]
    public class RequestController : Controller
    {
        private readonly IMediator _mediator;


        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<ActionResult<RequestDTO>> Add(CreateRequestDTO createRequestDTO)
        {
            var command = new Add<Request, CreateRequestDTO, RequestDTO>(createRequestDTO);
            var response = await _mediator.Send(command);

            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
    }
}