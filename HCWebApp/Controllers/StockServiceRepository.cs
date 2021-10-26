using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HCWebService.Controllers;

namespace HCWebApp.Controllers
{
    public class StockServiceRepository : IStockServiceRepository
    {
        public async Task<List<Quote>> GetQuotes()
        {
            var url = "https://localhost:44335/stock";
            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
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
            }
            catch (Exception)
            {

            }
            return new List<Quote>();
        }
    }
}