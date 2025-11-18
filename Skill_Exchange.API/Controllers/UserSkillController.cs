using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.UserSkill.Commands;
using Skill_Exchange.Application.Services.UserSkill.Queries;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserSkillController : Controller
    {
        public IMediator _mediator;
        public UserSkillController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //-------------------------------------------------------------------------//
        //                            Get end points                               //
        //-------------------------------------------------------------------------//
        [HttpGet("api/all")]
        public async Task<ActionResult<IEnumerable<UserSkillDTO>>> GeTAll()
        {
            var query = new GetAll<UserSkills, UserSkillsDTO>(null);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        [HttpGet("api/users-by-skill/{skillId:Guid}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersBySkill([FromRoute] Guid skillId, [FromQuery] GetUsersBySkillDTO getUsersBySkillDTO)
        {
            getUsersBySkillDTO.SkillId = skillId;
            var query = new GetUsersBySkill(getUsersBySkillDTO);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
        [HttpGet("api/user_skill/{userId:Guid}/{skillId:Guid}")]
        public async Task<ActionResult<UserSkillDTO>> GetUserSkill([FromRoute] Guid userId, [FromRoute] Guid skillId)
        {
            var query = new GetUserSkill(new GetUserSkillDTO { UserId = userId, SkillId = skillId });
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
        [HttpGet("api/user_skills/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<UserSkillDTO>>> GetUserSkills([FromRoute] Guid userId, [FromQuery] GetUserSkillsDTO getUserSkillsDTO)
        {
            getUserSkillsDTO.UserId = userId;
            var query = new GetUserSkills(getUserSkillsDTO);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        //-------------------------------------------------------------------------//
        //                            Post Endpoints                               //
        //-------------------------------------------------------------------------//
        [HttpPost("/add")]
        public async Task<ActionResult<UserSkillDTO>> AddUserSkill([FromBody] AddUserSkillDTO addUserSkillDTO)
        {
            var command = new Add<UserSkills, AddUserSkillDTO, UserSkillDTO>(addUserSkillDTO);
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }



        //-------------------------------------------------------------------------//
        //                            Delte Endpoints                              //
        //-------------------------------------------------------------------------//
        [HttpDelete("api/delte-user-skill")]
        public async Task<ActionResult<bool>> DeleteSkillFromUser(DeleteUserSkillDTO deleteUserSkillDTO)
        {
            var command = new DeleteUserSkill(deleteUserSkillDTO);
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
    }
}