using Acme.UserProfileService.Application.DTOs;
using Acme.UserProfileService.Application.Interfaces;
using Acme.UserProfileService.Domain;

namespace Acme.UserProfileService.Application.Services
{
    public class UserProfileServices : IUserProfileService
    {
        private readonly IUserProfileRepository _repo;

        public UserProfileServices(IUserProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> CreateAsync(UserProfileDto dto)
        {
            var user = new UserProfile(dto.Name!, dto.Email!, dto.Age);
            await _repo.AddAsync(user);
            return user.Id;
        }

        public async Task<UserProfileDto?> GetByIdAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user is null) return null;

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age
            };
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();

            return users.Select(user => new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age
            });
        }
    }
}
