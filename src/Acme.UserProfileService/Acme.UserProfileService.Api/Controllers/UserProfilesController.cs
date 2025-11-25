using Acme.UserProfileService.Api.DTOs;
using Acme.UserProfileService.Application.DTOs;
using Acme.UserProfileService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acme.UserProfileService.Api.Controllers
{

    [ApiController]
    [Route("api/userprofiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _service;

        public UserProfilesController(IUserProfileService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserProfileApiDto dto)
        {
            var userProfileDto = new UserProfileDto()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age
            };
            
            return CreatedAtAction(nameof(GetById), new { id = userProfileDto.Id }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }
    }


}
