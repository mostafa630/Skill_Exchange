using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.User.Commands;
using Skill_Exchange.Application.Services.UserSkill.Commands;
using Skill_Exchange.Application.Services.UserSkill.Queries;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("api/users-by-skill")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersBySkill([FromQuery] GetUsersBySkillDTO getUsersBySkillDTO)
        {
            var query = new GetUsersBySkill(getUsersBySkillDTO);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
        [HttpGet("api/user_skill")]
        public async Task<ActionResult<UserSkillDTO>> GetUserSkill(GetUserSkillDTO getUserSkillDTO)
        {
            var query = new GetUserSkill(getUserSkillDTO);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
        [HttpGet("api/user_skills")]
        public async Task<ActionResult<IEnumerable<UserSkillDTO>>> GetUserSkills(GetUserSkillsDTO getUserSkillsDTO)
        {
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