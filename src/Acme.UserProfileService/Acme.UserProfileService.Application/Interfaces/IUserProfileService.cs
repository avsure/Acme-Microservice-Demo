using Acme.UserProfileService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.UserProfileService.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<Guid> CreateAsync(UserProfileDto dto);
        Task<UserProfileDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserProfileDto>> GetAllAsync();
    }
}
