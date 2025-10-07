using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalCommands.Handlers;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
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
            .AddRequestHandlers();
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
            //TODO
            return services;
        }
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<Add<Request, CreateRequestDTO, RequestDTO>, Result<RequestDTO>>, AddHandler<Request, CreateRequestDTO, RequestDTO>>();
            services.AddTransient<IRequestHandler<GetAll<Request, RequestDTO>, Result<IEnumerable<RequestDTO>>>, GetAllHandler<Request, RequestDTO>>();
            return services;
        }

    }
}
