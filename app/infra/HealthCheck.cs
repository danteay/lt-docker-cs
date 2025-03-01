using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace app.infra
{
    public class CustomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            // Add your health check logic here
            var isHealthy = true; // Replace with actual health check logic

            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("The service is healthy."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("The service is unhealthy."));
        }
    }
}
