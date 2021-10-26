using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HCWebService.Controllers
{
    public interface IStockRepository
    {
        Task<List<Quote>> GetQuotes();
    }

    class StockRepository : IStockRepository
    {
        private readonly IConfiguration _configuration;

        public StockRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Quote>> GetQuotes()
        {
            string sql = "SELECT * FROM StockQuotes";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("database")))
            {
                var articles = await connection.QueryAsync<Quote>(sql);
                return articles.ToList();
            }
        }
    }
}