using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Application.DTOs
{
    public class RecommendationDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
