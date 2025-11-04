using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Notifications;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Application.DTOs.Skill;
using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Application.Services.Conversation.Queries;
using Skill_Exchange.Application.Services.Conversation.Queries.Handlers;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalCommands.Handlers;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
using Skill_Exchange.Application.Services.Notifications.Commands;
using Skill_Exchange.Application.Services.Notifications.Commands.Handlers;
using Skill_Exchange.Application.Services.Notifications.Queries;
using Skill_Exchange.Application.Services.Notifications.Queries.Handlers;
using Skill_Exchange.Application.Services.RatingAndFeedback;
using Skill_Exchange.Application.Services.RatingAndFeedback.Commands;
using Skill_Exchange.Application.Services.RatingAndFeedback.Commands.Handlers;
using Skill_Exchange.Application.Services.RatingAndFeedback.Handlers;
using Skill_Exchange.Application.Services.RatingAndFeedback.Queries;
using Skill_Exchange.Application.Services.RatingAndFeedback.Queries.Handlers;
using Skill_Exchange.Application.Services.Skill.Commands;
using Skill_Exchange.Application.Services.Skill.Commands.Handlers;
using Skill_Exchange.Application.Services.SkillCategory.Commands;
using Skill_Exchange.Application.Services.SkillCategory.Commands.Handlers;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API
{
    public static class MediatorExtension
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services)
        {
            services
            .AddUserHandlers()
            .AddSkillHandlers()
            .AddSkillCategoryHandlers()
            .AddRequestHandlers()
            .AddNotificationHandlers()
            .AddUserSkillHandlers()
            .AddRatingAndFeedbackHandlers();
            return services;
        }
        public static IServiceCollection AddUserHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<AppUser, UserDTO>, Result<IEnumerable<UserDTO>>>, GetAllHandler<AppUser, UserDTO>>();
            services.AddTransient<IRequestHandler<GetById<AppUser, UserDTO>, Result<UserDTO>>, GetByIdHandler<AppUser, UserDTO>>();
            services.AddTransient<IRequestHandler<Add<AppUser, CreateUserDTO, CreateUserResponseDTO>, Result<CreateUserResponseDTO>>, AddHandler<AppUser, CreateUserDTO, CreateUserResponseDTO>>();
            return services;
        }
        private static IServiceCollection AddSkillCategoryHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<SkillCategory, SkillCategoryDTO>, Result<IEnumerable<SkillCategoryDTO>>>, GetAllHandler<SkillCategory, SkillCategoryDTO>>();
            services.AddTransient<IRequestHandler<GetById<SkillCategory, SkillCategoryDTO>, Result<SkillCategoryDTO>>, GetByIdHandler<SkillCategory, SkillCategoryDTO>>();
            services.AddTransient<IRequestHandler<Add<SkillCategory, SkillCategoryDTO, SkillCategoryDTO>, Result<SkillCategoryDTO>>, AddHandler<SkillCategory, SkillCategoryDTO, SkillCategoryDTO>>();
            services.AddTransient<IRequestHandler<UpdateSkillCategory, Result<string>>, UpdateSkillCategoryHandler>();
            services.AddTransient<IRequestHandler<Delete<SkillCategory>, Result<string>>, DeleteHandler<SkillCategory>>();
            return services;
        }
        public static IServiceCollection AddSkillHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<Skill, SkillResponseDto>, Result<IEnumerable<SkillResponseDto>>>, GetAllHandler<Skill, SkillResponseDto>>();
            services.AddTransient<IRequestHandler<GetById<Skill, SkillResponseDto>, Result<SkillResponseDto>>, GetByIdHandler<Skill, SkillResponseDto>>();
            services.AddTransient<IRequestHandler<Add<Skill, CreateSkillDto, Skill>, Result<Skill>>, AddHandler<Skill, CreateSkillDto, Skill>>();
            services.AddTransient<IRequestHandler<UpdateSkill, Result<string>>, UpdateSkillHandler>();
            services.AddTransient<IRequestHandler<Delete<Skill>, Result<string>>, DeleteHandler<Skill>>();
            return services;
        }
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<Add<Request, CreateRequestDTO, RequestDTO>, Result<RequestDTO>>, AddHandler<Request, CreateRequestDTO, RequestDTO>>();
            services.AddTransient<IRequestHandler<GetAll<Request, RequestDTO>, Result<IEnumerable<RequestDTO>>>, GetAllHandler<Request, RequestDTO>>();
            services.AddTransient<IRequestHandler<Delete<Request>, Result<string>>, DeleteHandler<Request>>();

            return services;
        }
        public static IServiceCollection AddUserSkillHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<Add<UserSkills, AddUserSkillDTO, UserSkillDTO>, Result<UserSkillDTO>>, AddHandler<UserSkills, AddUserSkillDTO, UserSkillDTO>>();
            services.AddTransient<IRequestHandler<GetAll<UserSkills, UserSkillsDTO>, Result<IEnumerable<UserSkillsDTO>>>, GetAllHandler<UserSkills, UserSkillsDTO>>();
            return services;
        }
        public static IServiceCollection AddNotificationHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<Notification, NotificationDto>, Result<IEnumerable<NotificationDto>>>, GetAllHandler<Notification, NotificationDto>>();
            services.AddTransient<IRequestHandler<GetById<Notification, NotificationDto>, Result<NotificationDto>>, GetByIdHandler<Notification, NotificationDto>>();
            services.AddTransient<IRequestHandler<Add<Notification, CreateNotificationDto, NotificationDto>, Result<NotificationDto>>, AddHandler<Notification, CreateNotificationDto, NotificationDto>>();
            services.AddTransient<IRequestHandler<Delete<Notification>, Result<string>>, DeleteHandler<Notification>>();
            services.AddTransient<IRequestHandler<GetNotificationsByUserId, Result<List<NotificationDto>>>, GetNotificationsByUserIdHandler>();
            //services.AddTransient<IRequestHandler<DeleteUserNotification, Result<string>>, DeleteUserNotificationHandler>();
            services.AddTransient<IRequestHandler<DeleteSpecificUserNotification, Result<string>>, DeleteSpecificUserNotificationHandler>();
            services.AddTransient<IRequestHandler<UpdateNotification, Result<string>>, UpdateNotificationHandler>();

            return services;
        }
        // ---------------- RATING & FEEDBACK ----------------
        private static IServiceCollection AddRatingAndFeedbackHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<RatingAndFeedback, RatingDetailsDto>, Result<IEnumerable<RatingDetailsDto>>>, GetAllHandler<RatingAndFeedback, RatingDetailsDto>>();
            services.AddTransient<IRequestHandler<GetById<RatingAndFeedback, RatingDetailsDto>, Result<RatingDetailsDto>>, GetByIdHandler<RatingAndFeedback, RatingDetailsDto>>();
            services.AddTransient<IRequestHandler<Add<RatingAndFeedback, AddRatingAndFeedbackDto, RatingDetailsDto>, Result<RatingDetailsDto>>, AddHandler<RatingAndFeedback, AddRatingAndFeedbackDto, RatingDetailsDto>>();
            services.AddTransient<IRequestHandler<Delete<RatingAndFeedback>, Result<string>>, DeleteHandler<RatingAndFeedback>>();
            services.AddTransient<IRequestHandler<GetRatingsReceivedByUserQuery, Result<List<RatingReceivedByUserDto>>>, GetRatingsReceivedByUserHandler>();
            services.AddTransient<IRequestHandler<GetRatingsGivenByUserQuery, Result<List<RatingGivenByUserDto>>>, GetRatingsGivenByUserHandler>();
            services.AddTransient<IRequestHandler<UpdateRatingAndFeedback, Result<string>>, UpdateRatingAndFeedbackHandler>();
            services.AddTransient<IRequestHandler<GetUserRatingSummary, Result<UserRatingSummaryDto>>, GetUserRatingSummaryHandler>();

            return services;
        }
        // ---------------- Conversation ----------------
        public static IServiceCollection AddConversationHandlers(this IServiceCollection services)
        {
            // Conversation Queries
            services.AddTransient<IRequestHandler<GetConversation, Result<Guid>>, GetConversationHandler>();
            
            return services;
        }


    }
}
