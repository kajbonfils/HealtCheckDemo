using HCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HCWebApp.Controllers
{
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleRepository _articleRepository;
        private readonly IStockServiceRepository _stockService;

        public HomeController(ILogger<HomeController> logger, IArticleRepository articleRepository, IStockServiceRepository stockService)
        {
            _logger = logger;
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeModel
            {
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

    public class ArticleRepository : IArticleRepository
    {
        private readonly IConfiguration _configuration;

        public ArticleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Article>> GetArticles()
        {
            string sql = "SELECT * FROM Articles ORDER BY PublishingDate desc";
            try {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("database")))
            {
                var articles = await connection.QueryAsync<Article>(sql);
                return articles.ToList();
            }
            }
            catch (Exception){}

            return new List<Article>();
        }
    }
}
