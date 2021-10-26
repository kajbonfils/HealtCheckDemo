using System.Collections.Generic;
using System.Threading.Tasks;
using HCWebApp.Models;

namespace HCWebApp.Controllers
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetArticles();
    }
}
