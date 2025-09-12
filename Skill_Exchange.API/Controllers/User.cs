using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.User.Queries;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {

        private IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /*[HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAll<AppUser, UserDTO>();
            var users = await mediator.Send(query);
            return Ok(users);
        }*/

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetById<AppUser, UserDTO>(id);
            var users = await mediator.Send(query);
            return Ok(users);
        }



    }
}