using Acme.UserProfileService.Application.Interfaces;
using Acme.UserProfileService.Domain;

namespace Acme.UserProfileService.Infrastructure.Repositories
{
    public class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _db = new()
                {
                    new UserProfile( "Anita", "anita@example.com", 35),
                    new UserProfile( "John", "john@example.com", 30),
                    new UserProfile( "Riya", "riya@example.com", 28),
                };

        public Task AddAsync(UserProfile profile)
        {
            _db.Add(profile);
            return Task.CompletedTask;
        }

        public Task<UserProfile?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_db.FirstOrDefault(x => x.Id == id));
        }

        public Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<UserProfile>>(_db);
        }
    }
}
