using Acme.Contracts;
using Acme.UserProfileService.Api.DTOs;
using Acme.UserProfileService.Application.DTOs;
using Acme.UserProfileService.Application.Interfaces;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;

namespace Acme.UserProfileService.Api.Controllers
{

    [ApiController]
    [Route("api/userprofiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _service;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserProfilesController(IUserProfileService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserProfileApiDto dto)
        {
            var id = await _service.CreateAsync(new UserProfileDto
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Age = dto.Age
            });

            var userprofile = await _service.GetByIdAsync(id);
          
            await _publishEndpoint.Publish<IUserCreated>(new
            {
                UserId = userprofile.Id,
                Email = userprofile.Email,
                DisplayName = userprofile.Name,
                CreatedAt = DateTime.UtcNow
            });

            return CreatedAtAction(nameof(GetById), new { id = userprofile.Id }, null);
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
