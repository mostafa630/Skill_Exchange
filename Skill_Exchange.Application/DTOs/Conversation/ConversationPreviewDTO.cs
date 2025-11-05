namespace Skill_Exchange.Application.DTOs.Conversation
{
    public class ConversationPreviewDTO
    {
        public Guid ConversationId { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageSentAt { get; set; }
        public List<Guid> Participants { get; set; } = new List<Guid>(); 
    }

}
