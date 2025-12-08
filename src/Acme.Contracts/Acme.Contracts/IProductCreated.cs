using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Contracts
{
    public interface IProductCreated
    {
        Guid ProductId { get; }
        string Name { get; }
        decimal Price { get; }
        DateTime CreatedAt { get; }
    }
}
