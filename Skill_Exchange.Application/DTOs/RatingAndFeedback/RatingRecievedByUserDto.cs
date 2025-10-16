using System;

namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class RatingReceivedByUserDto
    {
        public int? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? FromUserId { get; set; }        // giver
        public string? FromUserName { get; set; }    // giver name
    }
}
