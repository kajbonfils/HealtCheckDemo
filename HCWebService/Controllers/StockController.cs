using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HCWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
        }
        public async Task<IActionResult> Index()
        {
            var quotes = await _stockRepository.GetQuotes();
            return Ok(quotes);

        }
    }

    public class Quote
    {
        public int Id { get; set; }
        public string StockName { get; set; }
        public decimal StockPrice { get; set; }
    }
}
