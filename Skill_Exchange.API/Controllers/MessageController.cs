using Microsoft.AspNetCore.Mvc;
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
            var message = await _messageService.SendMessageAsync(
                request.SenderId, request.ReceiverId, request.Content, request.ConversationId);
            return Ok(message);
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetConversationMessages(Guid conversationId)
        {
            var messages = await _messageService.GetConversationMessagesAsync(conversationId);
            return Ok(messages);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMessages(Guid userId)
        {
            var messages = await _messageService.GetUserMessagesAsync(userId);
            return Ok(messages);
        }

        [HttpPut("{messageId}")]
        public async Task<IActionResult> UpdateMessage(Guid messageId, [FromBody] string newContent)
        {
            var success = await _messageService.UpdateMessageAsync(messageId, newContent);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            var success = await _messageService.DeleteMessageAsync(messageId);
            return success ? Ok() : NotFound();
        }
    }

    //public record SendMessageRequest(Guid SenderId, Guid ReceiverId, Guid ConversationId, string Content);
    //public record UpdateMessageRequest(string NewContent);
}
