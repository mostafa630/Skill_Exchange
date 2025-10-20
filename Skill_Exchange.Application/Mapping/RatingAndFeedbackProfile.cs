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
               /* .ForMember(dest => dest.FromUserName,
                           opt => opt.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}".Trim()))
                .ForMember(dest => dest.ToUserName,
                           opt => opt.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}".Trim()))*/
                .ReverseMap();

            // Ratings given by user
            CreateMap<RatingAndFeedback, RatingGivenByUserDto>()
                .ForMember(dest => dest.ToUserName,
                           opt => opt.MapFrom(src => $"{src.ToUser.FirstName} {src.ToUser.LastName}".Trim()));

            // Ratings received by user
            CreateMap<RatingAndFeedback, RatingReceivedByUserDto>()
                .ForMember(dest => dest.FromUserName,
                           opt => opt.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}".Trim()));

            // Individual user feedback mapping
            CreateMap<RatingAndFeedback, UserFeedbackDto>()
                .ForMember(dest => dest.FromUserName,
                    opt => opt.MapFrom(src => $"{src.FromUser.FirstName} {src.FromUser.LastName}".Trim()))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.Feedback))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // User rating summary
            CreateMap<List<RatingAndFeedback>, UserRatingSummaryDto>()
                .ConvertUsing(src => new UserRatingSummaryDto
                {
                    AverageScore = src.Any(r => r.Score.HasValue)
                        ? Math.Round(src.Where(r => r.Score.HasValue).Average(r => r.Score!.Value), 2)
                        : 0,
                    TotalRatings = src.Count,
                    Feedbacks = src.Select(r => new UserFeedbackDto
                    {
                        FromUserName = $"{r.FromUser.FirstName} {r.FromUser.LastName}".Trim(),
                        Score = r.Score,
                        Feedback = r.Feedback,
                        CreatedAt = r.CreatedAt
                    }).ToList()
                });

        }
    }
}
