using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill;
using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalCommands.Handlers;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
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
            .AddSkillCategoryHandlers();
            return services;
        }
        public static IServiceCollection AddUserHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<AppUser, UserDTO>, Result<IEnumerable<UserDTO>>>, GetAllHandler<AppUser, UserDTO>>();
            services.AddTransient<IRequestHandler<GetById<AppUser, UserDTO>, Result<UserDTO>>, GetByIdHandler<AppUser, UserDTO>>();
            services.AddTransient<IRequestHandler<Add<AppUser, CreateUserDTO, CreateUserResponseDTO>, CreateUserResponseDTO>, AddHandler<AppUser, CreateUserDTO, CreateUserResponseDTO>>();
            return services;
        }
        private static IServiceCollection AddSkillCategoryHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<SkillCategory, SkillCategoryDTO>, Result<IEnumerable<SkillCategoryDTO>>>, GetAllHandler<SkillCategory, SkillCategoryDTO>>();
            services.AddTransient<IRequestHandler<GetById<SkillCategory, SkillCategoryDTO>, Result<SkillCategoryDTO>>, GetByIdHandler<SkillCategory, SkillCategoryDTO>>();
            services.AddTransient<IRequestHandler<Add<SkillCategory, SkillCategoryDTO, SkillCategoryDTO>, SkillCategoryDTO>, AddHandler<SkillCategory, SkillCategoryDTO, SkillCategoryDTO>>();
            services.AddTransient<IRequestHandler<UpdateSkillCategory, Result<string>>, UpdateSkillCategoryHandler>();
            services.AddTransient<IRequestHandler<Delete<SkillCategory>, Result<string>>, DeleteHandler<SkillCategory>>();
            return services;
        }
        public static IServiceCollection AddSkillHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<Skill, SkillResponseDto>, Result<IEnumerable<SkillResponseDto>>>, GetAllHandler<Skill, SkillResponseDto>>();
            services.AddTransient<IRequestHandler<GetById<Skill, SkillResponseDto>, Result<SkillResponseDto>>, GetByIdHandler<Skill, SkillResponseDto>>();
            services.AddTransient<IRequestHandler<Add<Skill, CreateSkillDto, Skill>, Skill>, AddHandler<Skill, CreateSkillDto, Skill>>();
            services.AddTransient<IRequestHandler<UpdateSkill, Result<string>>, UpdateSkillHandler>();
            services.AddTransient<IRequestHandler<Delete<Skill>, Result<string>>, DeleteHandler<Skill>>();
            return services;
        }


    }
}
