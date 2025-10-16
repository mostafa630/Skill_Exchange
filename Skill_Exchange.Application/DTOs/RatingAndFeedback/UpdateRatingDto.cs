using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.RatingAndFeedback
{
    public class UpdateRatingDto
    {
        public int? Score { get; set; }
        public string? Feedback { get; set; }
    }
}
