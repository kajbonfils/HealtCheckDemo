using HCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace HCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IStockServiceRepository _stockService;
        private readonly HealthCheckService _healthCheckService;

        public HomeController(IArticleRepository articleRepository, IStockServiceRepository stockService, HealthCheckService healthCheckService)
        {
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
            _healthCheckService = healthCheckService;
        }

        public async Task<IActionResult> Index()
        {
            var health = await _healthCheckService.CheckHealthAsync();
            var json = "";// System.Text.Json.JsonSerializer.Serialize(health, new JsonSerializerOptions() { WriteIndented = true, MaxDepth=600 });
            var model = new HomeModel
            {
                Health = health,
                HealthJson = json,
                Articles = await _articleRepository.GetArticles(),
                Quotes = await _stockService.GetQuotes()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
