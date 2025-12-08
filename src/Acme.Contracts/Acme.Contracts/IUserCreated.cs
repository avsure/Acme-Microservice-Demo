using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Contracts
{
    public interface IUserCreated
    {
        Guid UserId { get; }
        string Email { get; }
        string DisplayName { get; }
        DateTime CreatedAt { get; }
    }
}
