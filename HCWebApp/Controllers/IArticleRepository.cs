using HCWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCWebApp.Controllers
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetArticles();
    }
}
