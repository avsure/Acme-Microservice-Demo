using Acme.UserProfileService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.UserProfileService.Application.Interfaces
{
    public interface IUserProfileRepository
    {
        Task AddAsync(UserProfile userProfile);
        Task<UserProfile?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserProfile>> GetAllAsync();
    }
}
