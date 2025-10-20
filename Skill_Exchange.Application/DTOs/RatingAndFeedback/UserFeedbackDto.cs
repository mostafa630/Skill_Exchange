namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class UserFeedbackDto
    {
        public string FromUserName { get; set; }
        public double? Score { get; set; }
        public string Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
