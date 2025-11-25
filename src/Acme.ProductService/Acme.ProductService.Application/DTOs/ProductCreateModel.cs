using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Application.DTOs
{
    public record ProductCreateModel(string Name, 
                                     decimal Price, 
                                     string Category, 
                                     string Description);

}
