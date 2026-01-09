using Acme.Contracts;
using Acme.UserProfileService.Api.DTOs;
using Acme.UserProfileService.Application.DTOs;
using Acme.UserProfileService.Application.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Acme.UserProfileService.Api.Controllers
{

    [ApiController]
    [Route("api/userprofiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _service;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UserProfilesController> _logger;

        public UserProfilesController(IUserProfileService service,
                               IPublishEndpoint publishEndpoint,
                              ILogger<UserProfilesController> logger)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
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

            _logger.LogInformation("Created user profile {UserId} with email {Email}", userprofile.Id, userprofile.Email);

            return CreatedAtAction(nameof(GetById), new { id = userprofile.Id }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetByIdAsync(id);

            _logger.LogInformation(user is null
                ? "User profile with ID {UserId} not found"
                : "Retrieved user profile {UserId} with email {Email}",
                id, user?.Email);

            return user is null ? NotFound() : Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("User: GetAll test log");

            Serilog.Log.Information("Serilog static test log");

            var users = await _service.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} user profiles", users.Count());   

            return Ok(users);
        }
    }
}
