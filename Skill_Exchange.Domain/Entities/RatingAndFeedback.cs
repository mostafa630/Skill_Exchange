using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Entities
{
    public class RatingAndFeedback
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Range(1, 5)]
        public int Score { get; set; }
        public string Feedback { get; set; }

    }
}