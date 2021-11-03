using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HCWebApp
{
    public class ServiceHealthCheck : IHealthCheck
    {
        private Uri _serviceHealthUri;
        private readonly HealthCheckResult _failureStatus;

        public ServiceHealthCheck(Uri serviceHealthUri, HealthCheckResult failureStatus)
        {
            _serviceHealthUri = serviceHealthUri;
            this._failureStatus = failureStatus;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {

                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler) { Timeout = TimeSpan.FromMilliseconds(200) })
                    {
                        var result = await client.GetAsync(_serviceHealthUri); ;

                        if (result.IsSuccessStatusCode)
                        {
                            return HealthCheckResult.Healthy(_serviceHealthUri.ToString(), null);
                        }
                    }
                }

            }
            catch {}
            var failureStatus = new HealthCheckResult(_failureStatus.Status, _serviceHealthUri.ToString(), null, null);
            return failureStatus;
        }

    }
}
