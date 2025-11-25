using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.RecommendationService.Domain.Entities
{
    public class Recommendation
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string Message { get; private set; }

        private Recommendation() { }

        public Recommendation(Guid id, Guid productId, string message)
        {
            Id = id;
            ProductId = productId;
            Message = message;
        }
    }
}
