using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;

namespace Skill_Exchange.Application.Services.User.Queries
{
    public record GetUserById(string id) : IRequest<Result<UserDTO>>;
}