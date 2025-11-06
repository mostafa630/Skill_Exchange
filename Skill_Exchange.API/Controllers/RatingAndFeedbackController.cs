using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.RatingAndFeedback;
using Skill_Exchange.Application.Services.RatingAndFeedback.Commands;
using Skill_Exchange.Application.Services.RatingAndFeedback.Queries;
using Skill_Exchange.Domain.Entities;

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

        /// <summary>
        /// for testing (to see that the last feedback added)
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRatings()
        {
            var result = await _mediator.Send(new GetAll<RatingAndFeedback, RatingDetailsDto>(null));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Get rating by ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRatingById(Guid id)
        {
            var result = await _mediator.Send(new GetById<RatingAndFeedback, RatingDetailsDto>(id));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Get ratings received by a user
        [HttpGet("received/{userId:guid}")]
        public async Task<IActionResult> GetRatingsReceivedByUser(Guid userId)
        {
            var result = await _mediator.Send(new GetRatingsReceivedByUserQuery(userId));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data); // List<RatingReceivedByUserDto>
        }

        // Get ratings given by a user
        [HttpGet("given/{userId:guid}")]
        public async Task<IActionResult> GetRatingsGivenByUser(Guid userId)
        {
            var result = await _mediator.Send(new GetRatingsGivenByUserQuery(userId));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data); // List<RatingGivenByUserDto>
        }

        // Get user rating summary
        [HttpGet("summary/{userId:guid}")]
        public async Task<IActionResult> GetUserRatingSummary(Guid userId)
        {
            var result = await _mediator.Send(new GetUserRatingSummary(userId));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data); // UserRatingSummaryDto
        }

        // Add a new rating
        [HttpPost("add")]
        public async Task<IActionResult> AddRating([FromBody] AddRatingAndFeedbackDto dto)
        {
            var result = await _mediator.Send(new Add<RatingAndFeedback, AddRatingAndFeedbackDto, RatingDetailsDto>(dto));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Rating added successfully");
        }

        // Update an existing rating
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateRating(Guid id, [FromBody] UpdateRatingDto dto)
        {
            var result = await _mediator.Send(new UpdateRatingAndFeedback(id, dto));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Rating updated successfully");
        }

        // Delete a rating
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteRating(Guid id)
        {
            var result = await _mediator.Send(new Delete<RatingAndFeedback>(id));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok("Rating deleted successfully");
        }

    }
}
