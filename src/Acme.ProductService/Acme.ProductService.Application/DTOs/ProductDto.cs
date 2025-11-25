using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Application.DTOs
{
    public record ProductDto(
     Guid Id,
     string Name,
     string Category,
     decimal Price,
     string Description
 );
}
