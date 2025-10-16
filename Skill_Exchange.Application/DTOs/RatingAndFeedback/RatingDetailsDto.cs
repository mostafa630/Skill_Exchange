using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class RatingDetailsDto
    {
        public Guid Id { get; set; }
        public int? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? FromUserId { get; set; }
        public string? FromUserName { get; set; }

        public Guid? ToUserId { get; set; }
        public string? ToUserName { get; set; }
    }
}
