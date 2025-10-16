using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Commands
{
    public record UpdateRatingAndFeedback(UpdateRatingDto updateRatingDto): IRequest<Result<string>>;
}
