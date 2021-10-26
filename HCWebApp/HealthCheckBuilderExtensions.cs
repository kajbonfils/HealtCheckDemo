using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HCWebApp
{
    public static class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddServiceCheck(this IHealthChecksBuilder builder, Uri healthCheckUri, HealthCheckResult failureStatus) =>
            builder.Add(new HealthCheckRegistration("ServiceHealthCheck", new ServiceHealthCheck(healthCheckUri, failureStatus), null, null));
    }


}
