using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Application.Services;

namespace Skill_Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageDTO request)
        {
            var result = await _messageService.SendMessageAsync(
                request.SenderId, request.ReceiverId, request.Content);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetConversationMessages(Guid conversationId)
        {
            var result = await _messageService.GetConversationMessagesAsync(conversationId);

            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMessages(Guid userId)
        {
            var result = await _messageService.GetUserMessagesAsync(userId);

            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPut("{messageId}")]
        public async Task<IActionResult> UpdateMessage(Guid messageId, [FromBody] string newContent)
        {
            var result = await _messageService.UpdateMessageAsync(messageId, newContent);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(new { message = "Message updated successfully." });
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(new { message = "Message deleted successfully." });
        }
    }
}
