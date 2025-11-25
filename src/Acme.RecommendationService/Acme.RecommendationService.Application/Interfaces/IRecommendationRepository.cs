using Acme.RecommendationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Application.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<IEnumerable<Recommendation>> GetRecommendationsForProductAsync(Guid productId);
        Task AddAsync(Recommendation recommendation);
    }
}
