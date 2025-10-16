namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class AddRatingAndFeedbackDto
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; } 
        public int? Score { get; set; }
        public string? Feedback { get; set; }
    }
}
