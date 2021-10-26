using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HCWebService;
using HCWebService.Controllers;

namespace HCWebApp.Controllers
{
    public interface IStockServiceRepository
    {
        Task<List<Quote>> GetQuotes();
    }

    class StockServiceRepository : IStockServiceRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public StockServiceRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<List<Quote>> GetQuotes()
        {
            var url = "https://localhost:44335/stock";
            try
            {
                using var client = _clientFactory.CreateClient();
                var httpResult = await client.GetAsync(url);
                if (httpResult.IsSuccessStatusCode)
                {
                    var quotesJson = await httpResult.Content.ReadAsStringAsync();
                    var quotes = JsonSerializer.Deserialize<List<Quote>>(quotesJson,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return quotes;
                }

            }
            catch (Exception)
            {

            }
            return new List<Quote>();
        }
    }
}