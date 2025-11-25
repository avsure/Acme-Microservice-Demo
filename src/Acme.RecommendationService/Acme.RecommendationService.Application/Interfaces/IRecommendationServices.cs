using Acme.RecommendationService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Application.Interfaces
{
    public interface IRecommendationServices
    {
        Task<IEnumerable<RecommendationDto>> GetForProductAsync(Guid productId);
        Task<Guid> CreateRecommendationAsync(Guid productId, string message);
    }
}
