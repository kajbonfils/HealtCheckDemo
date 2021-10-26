using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HCWebService;
using HCWebService.Controllers;
using Microsoft.Extensions.Configuration;

namespace HCWebApp.Controllers
{
    public interface IStockServiceRepository
    {
        Task<List<Quote>> GetQuotes();
    }

    class StockServiceRepository : IStockServiceRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public StockServiceRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<List<Quote>> GetQuotes()
        {
            var url = _configuration.GetSection("API:HCService").Value;

            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using var client = new HttpClient(httpClientHandler) { Timeout = TimeSpan.FromMilliseconds(200) };

                    var httpResult = await client.GetAsync(url);
                    if (httpResult.IsSuccessStatusCode)
                    {
                        var quotesJson = await httpResult.Content.ReadAsStringAsync();
                        var quotes = JsonSerializer.Deserialize<List<Quote>>(quotesJson,
                            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        return quotes;
                    }
                }
            }
            catch (Exception)
            {

            }
            return new List<Quote>();
        }
    }
}