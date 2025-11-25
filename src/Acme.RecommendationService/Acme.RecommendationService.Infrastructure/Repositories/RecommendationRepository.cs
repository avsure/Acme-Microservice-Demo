using Acme.RecommendationService.Application.Interfaces;
using Acme.RecommendationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Infrastructure.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly List<Recommendation> _db = new()
        {
            new Recommendation(
                Guid.NewGuid(),
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "People who bought this also liked Wireless Mouse."
            ),

            new Recommendation(
                Guid.NewGuid(),
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Try Mechanical Keyboard — highly rated!"
            )
        };

        public Task<IEnumerable<Recommendation>> GetRecommendationsForProductAsync(Guid productId)
        {
            var result = _db.Where(x => x.ProductId == productId);
            return Task.FromResult<IEnumerable<Recommendation>>(result);
        }

        public Task AddAsync(Recommendation recommendation)
        {
            _db.Add(recommendation);
            return Task.CompletedTask;
        }
    }
}
