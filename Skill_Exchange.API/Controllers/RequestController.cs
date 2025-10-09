using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.Request.Queries;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly IMediator _mediator;


        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //-------------------------------------------------------------------------//
        //                            Get Endpoints                                //
        //-------------------------------------------------------------------------//
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<RequestDTO>>> GeTAll()
        {
            var query = new GetAll<Request, RequestDTO>(null);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        [HttpGet("sendedBy")]
        public async Task<ActionResult<IEnumerable<RequestDTO>>> GetRequestsSendedBy(string SenderId)
        {
            if (String.IsNullOrEmpty(SenderId.ToString()) || !Guid.TryParse(SenderId, out _))
            {
                return BadRequest("Invalid Id");
            }

            var query = new GetRequestsSended(Guid.Parse(SenderId));
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
        [HttpGet("receiveddBy")]
        public async Task<ActionResult<IEnumerable<RequestDTO>>> GetRequestsReceiveddBy(string RecieverId)
        {
            if (String.IsNullOrEmpty(RecieverId.ToString()) || !Guid.TryParse(RecieverId, out _))
            {
                return BadRequest("Invalid Id");
            }

            var query = new GetRequestsReceived(Guid.Parse(RecieverId));
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        //-------------------------------------------------------------------------//
        //                            Post Endpoints That get                       //
        //-------------------------------------------------------------------------//
        [HttpPost("between")]
        public async Task<ActionResult<RequestDTO>> GetRequestBetween(GetRequestBetweenDto betweenDto)
        {
            var query = new GetRequestBetween(betweenDto);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        //-------------------------------------------------------------------------//
        //                            Post Endpoints                               //
        //-------------------------------------------------------------------------//
        [HttpPost("add")]
        public async Task<ActionResult<RequestDTO>> Add(CreateRequestDTO createRequestDTO)
        {
            var command = new Add<Request, CreateRequestDTO, RequestDTO>(createRequestDTO);
            var response = await _mediator.Send(command);

            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

    }
}