using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Notifications;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.Notifications.Commands;
using Skill_Exchange.Application.Services.Notifications.Queries;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  1) Get all notifications
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAll<Notification, NotificationDto>(null));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        //  2) Get notification by Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetById<Notification, NotificationDto>(id));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        //  3) Add notification
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
        {
            var result = await _mediator.Send(new Add<Notification, CreateNotificationDto, NotificationDto>(dto));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Notification added successfully.");
        }

        //  4) Update notification
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateNotificationDto dto)
        {
            var result = await _mediator.Send(new UpdateNotification(dto));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Notification updated successfully.");
        }

        //  5) Delete notification by Id
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new Delete<Notification>(id));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Notification deleted successfully.");
        }

        //  6) Get notifications by user Id
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _mediator.Send(new GetNotificationsByUserId(userId));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        //  7) Delete a specific notification for a user
        [HttpDelete("user/{userId:guid}/notification/{notificationId:guid}")]
        public async Task<IActionResult> DeleteSpecificNotification(Guid userId, Guid notificationId)
        {
            var result = await _mediator.Send(new DeleteSpecificUserNotification(userId, notificationId));

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Notification deleted successfully.");
        }
    }
}
