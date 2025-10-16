using AutoMapper;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Mapping
{
    public class RatingAndFeedbackProfile : Profile
    {
        public RatingAndFeedbackProfile()
        {
            // Add/Update mappings
            CreateMap<RatingAndFeedback, AddRatingAndFeedbackDto>().ReverseMap();
            CreateMap<RatingAndFeedback, UpdateRatingDto>().ReverseMap();

            // Details mapping
            CreateMap<RatingAndFeedback, RatingDetailsDto>()
                .ForMember(dest => dest.FromUserName,
                           opt => opt.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}".Trim()))
                .ForMember(dest => dest.ToUserName,
                           opt => opt.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}".Trim()))
                .ReverseMap();

            // Ratings given by user
            CreateMap<RatingAndFeedback, RatingGivenByUserDto>()
                .ForMember(dest => dest.ToUserName,
                           opt => opt.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}".Trim()));

            // Ratings received by user
            CreateMap<RatingAndFeedback, RatingReceivedByUserDto>()
                .ForMember(dest => dest.FromUserName,
                           opt => opt.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}".Trim()));
        }
    }
}
