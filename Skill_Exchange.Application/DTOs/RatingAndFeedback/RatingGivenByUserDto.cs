using System;

namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class RatingGivenByUserDto
    {
        public int? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? ToUserId { get; set; }          // receiver
        public string? ToUserName { get; set; }      // receiver name
    }
}
