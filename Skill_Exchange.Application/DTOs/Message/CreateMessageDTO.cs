namespace Skill_Exchange.Application.DTOs.Message
{
    public class CreateMessageDTO
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
