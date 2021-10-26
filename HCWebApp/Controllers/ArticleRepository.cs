using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using HCWebApp.Models;

namespace HCWebApp.Controllers
{
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
