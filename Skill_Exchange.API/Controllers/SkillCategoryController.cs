using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.SkillCategory.Commands;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SkillCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  GET: api/SkillCategory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAll<SkillCategory, SkillCategoryDTO>());
            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        //  GET: api/SkillCategory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetById<SkillCategory, SkillCategoryDTO>(id));
            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        //  POST: api/SkillCategory
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SkillCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _mediator.Send(new Add<SkillCategory, SkillCategoryDTO, SkillCategoryDTO>(dto));
            return Ok(created);
        }

        //  PUT: api/SkillCategory
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SkillCategoryDTO dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                return BadRequest("Invalid category data.");

            var result = await _mediator.Send(new UpdateSkillCategory(dto));

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Error);
        }

        // ✅ DELETE: api/SkillCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new Delete<SkillCategory>(id));

            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Error);
        }
    }
}
