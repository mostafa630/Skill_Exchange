
using Amazon.Runtime.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.User.Commands
{
    public record UploadImage(Guid userId, IFormFile image) : IRequest<Result<string>>;

}