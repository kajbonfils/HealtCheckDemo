using System.Collections.Generic;
using System.Threading.Tasks;
using HCWebService.Controllers;

namespace HCWebApp.Controllers
{
    public interface IStockServiceRepository
    {
        Task<List<Quote>> GetQuotes();
    }
}