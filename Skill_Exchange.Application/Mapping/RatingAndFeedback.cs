using AutoMapper;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
namespace Skill_Exchange.Application.Mapping
{
    public class RatingAndFeedback:Profile
    {
        public RatingAndFeedback()
        {
            CreateMap<RatingAndFeedback,AddRatingAndFeedbackDto>().ReverseMap();
            CreateMap<RatingAndFeedback,UpdateRatingDto>().ReverseMap();
            CreateMap<RatingAndFeedback,RatingDetailsDto>().ReverseMap();
            CreateMap<RatingAndFeedback,UserRatingsDto>().ReverseMap();

        }
    }
}
