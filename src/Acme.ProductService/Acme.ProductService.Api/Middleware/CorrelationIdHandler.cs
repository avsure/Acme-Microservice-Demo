using MassTransit;

namespace Acme.ProductService.Api.Middleware
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var correlationId =
                _httpContextAccessor.HttpContext?
                    .Items["X-Correlation-ID"]?
                    .ToString();

            if (!string.IsNullOrEmpty(correlationId) &&
                !request.Headers.Contains("X-Correlation-ID"))
            {
                request.Headers.Add("X-Correlation-ID", correlationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
