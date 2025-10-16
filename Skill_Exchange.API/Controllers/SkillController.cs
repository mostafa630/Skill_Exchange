using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill;
using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.Skill.Commands;
using Skill_Exchange.Domain.Entities;
using System;
using System.Linq;
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

        // Add a new skill
        [HttpPost("add")]
        public async Task<IActionResult> AddSkill([FromBody] CreateSkillDto dto)
        {
            // Check if skill with same name already exists
            var existingSkills = await _mediator.Send(new GetAll<Skill, SkillResponseDto>(null));
            if (existingSkills.Success && existingSkills.Data != null)
            {
                var exists = existingSkills.Data.Any(s =>
                    s.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));

                if (exists)
                    return BadRequest($"A skill with the name '{dto.Name}' already exists.");
            }

            // Validate category
            var category = await _mediator.Send(new GetById<SkillCategory, SkillCategoryDTO>(dto.SkillCategoryId));
            if (!category.Success || category.Data == null)
                return BadRequest("Invalid SkillCategoryId: Category not found.");

            // Add the skill
            var entity = await _mediator.Send(new Add<Skill, CreateSkillDto, Skill>(dto));
            if (entity == null)
                return BadRequest("Failed to add skill.");

            return Ok("Skill created successfully.");
        }

        // Get all skills
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSkills()
        {
            var result = await _mediator.Send(new GetAll<Skill, SkillResponseDto>(null));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Get skill by ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSkillById(Guid id)
        {
            var result = await _mediator.Send(new GetById<Skill, SkillResponseDto>(id));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        // Update a skill
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSkill([FromBody] UpdateSkillDto dto)
        {
            var result = await _mediator.Send(new UpdateSkill(dto));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Skill updated successfully.");
        }

        // Delete a skill by ID
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            var result = await _mediator.Send(new Delete<Skill>(id));
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok("Skill deleted successfully.");
        }
    }
}
