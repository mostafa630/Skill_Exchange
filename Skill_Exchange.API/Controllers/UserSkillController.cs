using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Application.Services.GlobalCommands;
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
        //                            Post Endpoints that get                      //
        //-------------------------------------------------------------------------//
        [HttpPost("/user_skill")]
        public async Task<ActionResult<UserSkillDTO>> GetUserSkill([FromBody] GetUserSkillDTO getUserSkillDTO)
        {
            var query = new GetUserSkill(getUserSkillDTO);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }
         [HttpPost("/user_skills")]
        public async Task<ActionResult<IEnumerable<UserSkillDTO>>> GetUserSkills([FromBody] GetUserSkillsDTO getUserSkillsDTO)
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

    }
}