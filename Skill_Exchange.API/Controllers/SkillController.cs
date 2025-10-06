using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.Skill.Commands;
using Skill_Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skill_Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SkillController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  Add Skill
        [HttpPost]
        public async Task<IActionResult> AddSkill([FromBody] CreateSkillDto dto)
        {
            var result = await _mediator.Send(new Add<Skill, CreateSkillDto, Skill>(dto));
            result.CreatedBy = "system";
            return Ok(Result<Skill>.Ok(result));
        }

        //  Get All Skills
        [HttpGet]
        public async Task<IActionResult> GetAllSkills()
        {
            var result = await _mediator.Send(new GetAll<Skill, Skill>(null));
            if (!result.Success)
                return BadRequest(result.Error);
            return Ok(result.Data);
        }

        //  Get Skill by ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSkillById(Guid id)
        {
            var result = await _mediator.Send(new GetById<Skill, Skill>(id));
            if (!result.Success)
                return NotFound(result.Error);
            return Ok(result.Data);
        }

        //  Update Skill (you already have UpdateSkillHandler)
        [HttpPut]
        public async Task<IActionResult> UpdateSkill([FromBody] UpdateSkillDto dto)
        {
            var result = await _mediator.Send(new UpdateSkill(dto));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Error);
        }

        //  Delete Skill (using global handler)
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            var result = await _mediator.Send(new Delete<Skill>(id));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Error);
        }
    }
}
