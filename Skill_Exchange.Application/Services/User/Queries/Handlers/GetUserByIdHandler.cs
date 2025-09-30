using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserById, Result<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public GetUserByIdHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Result<UserDTO>> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(request.id);
                if (user == null)
                {
                    return Result<UserDTO>.Fail("No user Exists");
                }
                var userDto = _mapper.Map<UserDTO>(user);
                return Result<UserDTO>.Ok(userDto);
            }
            catch
            {
                return Result<UserDTO>.Fail("Operation Failed");
            }
        }
    }
}