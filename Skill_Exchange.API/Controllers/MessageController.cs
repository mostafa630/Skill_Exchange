using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Conversation;
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

        /// <summary>
        /// for testing (test adding messages in db)
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] CreateMessageDTO request)
        {
            var result = await _messageService.SendMessageAsync(
                request.SenderId, request.ReceiverId, request.Content);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }

        //  Get paginated messages in a conversation (for infinite scroll)
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetConversationMessages(
            Guid conversationId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _messageService.GetConversationMessagesPaginatedAsync(conversationId, page, pageSize);

            if (!result.Success)
                return NotFound(new { error = result.Error });

            return Ok(result.Data);
        }

        /// <summary>
        /// for testing (to see that the last messages added)
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMessages(
            Guid userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var pagination = new PaginationDto
            {
                ApplyPagination = true,
                Skip = (page - 1) * pageSize,
                Take = pageSize
            };

            var result = await _messageService.GetUserMessagesPaginatedAsync(userId, pagination);

            if (!result.Success)
                return NotFound(new { error = result.Error });

            return Ok(result.Data);
        }

        //  Update a message
        [HttpPut("{messageId}")]
        public async Task<IActionResult> UpdateMessage(Guid messageId, [FromBody] string Content)
        {
            var result = await _messageService.UpdateMessageAsync(messageId, Content);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { message = "Message updated successfully." });
        }

        //  Delete a message
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);

            if (!result.Success)
                return NotFound(new { error = result.Error });

            return Ok(new { message = "Message deleted successfully." });
        }
        /// <summary>
        /// hasn't been completed yes
        /// </summary>
        //  Get conversation previews (WhatsApp-style)
        [HttpGet("previews/{userId}")]
        public async Task<IActionResult> GetConversationPreviews(
            Guid userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _messageService.GetConversationPreviewsAsync(userId, page, pageSize);

            if (!result.Success)
                return NotFound(new { error = result.Error });

            return Ok(result.Data);
        }

    }
}
