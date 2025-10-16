using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.RatingAndFeedback.Commands;
using Skill_Exchange.Application.Services.RatingAndFeedback.Queries;
using Skill_Exchange.Domain.Entities;
using System.Threading.Tasks;

namespace Skill_Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingAndFeedbackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingAndFeedbackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ (GET) Get a specific rating by ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRatingById(Guid id)
        {
            var result = await _mediator.Send(new GetById<RatingAndFeedback, RatingDetailsDto>(id));
            if (!result.Success)
                return NotFound(result.Error);
            return Ok(result.Data);
        }

        // ✅ (GET) Get ratings received by a user
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetRatingsReceivedByUser(Guid userId)
        {
            var result = await _mediator.Send(new GetRatingsReceivedByUserQuery(userId));
            if (!result.Success)
                return NotFound(result.Error);
            return Ok(result.Data);
        }

        // ✅ (GET) Get ratings given by a user
        [HttpGet("from/{userId:guid}")]
        public async Task<IActionResult> GetRatingsGivenByUser(Guid userId)
        {
            var result = await _mediator.Send(new GetRatingsGivenByUserQuery(userId));
            if (!result.Success)
                return NotFound(result.Error);
            return Ok(result.Data);
        }

        // ✅ (POST) Add a new rating
        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] AddRatingAndFeedbackDto dto)
        {
            var result = await _mediator.Send(new Add<RatingAndFeedback, AddRatingAndFeedbackDto, RatingDetailsDto>(dto));
            if (!result.Success)
                return BadRequest(result.Error);
            return Ok(result.Data);
        }

        // ✅ (PUT) Update existing rating
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRating(Guid id, [FromBody] UpdateRatingDto dto)
        {
            dto.Id = id;
            var result = await _mediator.Send(new UpdateRatingAndFeedback(dto));
            if (!result.Success)
                return BadRequest(result.Error);
            return Ok(result.Error);
        }

        // ✅ (DELETE) Delete a rating
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            var result = await _mediator.Send(new Delete<RatingAndFeedback>(id));
            if (!result.Success)
                return NotFound(result.Error);
            return Ok(result.Error);
        }
    }
}
