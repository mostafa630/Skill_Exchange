using MediatR;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Prng.Drbg;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Interfaces;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.User.Commands;
using Skill_Exchange.Application.Services.User.Queries;
using Skill_Exchange.Application.Specifications;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("by_email/{email}")]

        //-------------------------------------------------------------------------//
        //                            Get Endpoints                                //
        //-------------------------------------------------------------------------//
        public async Task<ActionResult<UserDTO>> GetByEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid Email");
            }
            var query = new GetUserByEmail(email);
            var response = await _mediator.Send(query);

            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        [HttpGet("by_Id/{Id}")]
        public async Task<ActionResult<UserDTO>> GetById(string Id)
        {
            if (String.IsNullOrEmpty(Id.ToString()) || !Guid.TryParse(Id, out _))
            {
                return BadRequest("Invalid Id");
            }
            var query = new GetById<AppUser, UserDTO>(Guid.Parse(Id));
            var response = await _mediator.Send(query);

            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }


        [HttpGet("all/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll([FromRoute] Guid userId, [FromQuery] UserFilterDTO userFilterDTO, [FromQuery] UserIncludesDTO userIncludesDTO)
        {
            userFilterDTO.UserId = userId;
            var userSpec = UserSpecification.Build(userFilterDTO, userIncludesDTO);
            var query = new GetAll<AppUser, UserDTO>(userSpec);
            var response = await _mediator.Send(query);

            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        [HttpGet("are-friends/{user1Id:Guid}/{user2Id:Guid}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> AreUsersFriends([FromRoute] Guid user1Id, [FromRoute] Guid user2Id)
        {
            var query = new AreFriends(user1Id, user2Id);
            var response = await _mediator.Send(query);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        //-------------------------------------------------------------------------//
        //                            Put Endpoints                               //
        //-------------------------------------------------------------------------//

        [HttpPut("update/{email}")]
        public async Task<ActionResult<UserDTO>> Update([FromRoute] string email, [FromBody] UpdateUserDTO updateUserDTO)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            updateUserDTO.Email = email;
            var command = new UpdateUser(updateUserDTO);
            var response = await _mediator.Send(command);

            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        //-------------------------------------------------------------------------//
        //                            Post Endpoints                               //
        //-------------------------------------------------------------------------//
        [HttpPost("upload-image/{userId:Guid}")]
        public async Task<ActionResult<string>> UploadUserImage([FromRoute] Guid userId, IFormFile image)
        {
            var command = new UploadImage(userId, image);
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

        //-------------------------------------------------------------------------//
        //                            Delete Endpoints                             //
        //-------------------------------------------------------------------------//
        [HttpDelete("delete/{email}")]
        public async Task<ActionResult<bool>> Delete(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid Email");
            }
            var command = new DelteUser(email);
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response.Data) : BadRequest(response.Error);
        }

    }



}
