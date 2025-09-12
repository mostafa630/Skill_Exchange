using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API
{
    public static class MediatorExtension
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<GetAll<AppUser, UserDTO>, IEnumerable<UserDTO>>, GetAllHandler<AppUser, UserDTO>>();

            return services;
        }
    }
}
