using AutoMapper;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Application.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<CreateMessageDTO, Message>()
                .ForMember(dest => dest.SentAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

            CreateMap<Message, MessageResponseDTO>();
        }
    }
}
