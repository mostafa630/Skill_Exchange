using AutoMapper;
using Skill_Exchange.Application.DTOs.Conversation;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Domain.Entities;
using System.Collections.Generic;
using System;

namespace Skill_Exchange.Application.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            // Mapping for creating a new message
            CreateMap<CreateMessageDTO, Message>()
                .ForMember(dest => dest.SentAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.DeliveredAt, opt => opt.Ignore())
                .ForMember(dest => dest.ReadAt, opt => opt.Ignore());

            // Mapping from Message to MessageResponseDTO
            CreateMap<Message, MessageResponseDTO>();

            // Mapping for ConversationPreviewDTO
            CreateMap<(Guid ConversationId, Message LastMessage, List<Guid> Participants), ConversationPreviewDTO>()
                .ForMember(dest => dest.ConversationId, opt => opt.MapFrom(src => src.ConversationId))
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.LastMessage.Content))
                .ForMember(dest => dest.LastMessageSentAt, opt => opt.MapFrom(src => src.LastMessage.SentAt))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));
        }
    }
}
