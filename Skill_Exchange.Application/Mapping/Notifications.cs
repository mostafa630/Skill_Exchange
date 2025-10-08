using AutoMapper;
using Skill_Exchange.Application.DTOs.Notifications;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Application.Mapping
{
    public class Notifications : Profile
    {
        public Notifications()
        {
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();

        }
    }
}
