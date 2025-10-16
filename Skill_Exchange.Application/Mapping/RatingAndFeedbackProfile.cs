using AutoMapper;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Application.Mapping
{
    public class RatingAndFeedbackProfile:Profile
    {
        public RatingAndFeedbackProfile()
        {
            CreateMap<RatingAndFeedback,AddRatingAndFeedbackDto>().ReverseMap();
            CreateMap<RatingAndFeedback,UpdateRatingDto>().ReverseMap();
            CreateMap<RatingAndFeedback,RatingDetailsDto>().ReverseMap();
            CreateMap<RatingAndFeedback,UserRatingsDto>().ReverseMap();

        }
    }
}
