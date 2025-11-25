using Acme.RecommendationService.Application.DTOs;
using Acme.RecommendationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Application.Services
{
    public class RecommendationServices : IRecommendationServices
    {
        private readonly IRecommendationRepository _repo;

        public RecommendationServices(IRecommendationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RecommendationDto>> GetForProductAsync(Guid productId)
        {
            var recs = await _repo.GetRecommendationsForProductAsync(productId);

            return recs.Select(r => new RecommendationDto
            {
                Id = r.Id,
                Message = r.Message
            });
        }

        public async Task<Guid> CreateRecommendationAsync(Guid productId, string message)
        {
            var rec = new Domain.Entities.Recommendation(Guid.NewGuid(), productId, message);

            await _repo.AddAsync(rec);

            return rec.Id;
        }
    }
}
