namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class UserRatingSummaryDto
    {
        public Guid UserId { get; set; }
        public double AverageScore { get; set; }
        public int TotalRatings { get; set; }
        public List<UserFeedbackDto> Feedbacks { get; set; }
    }
}
