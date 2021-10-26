using System.Collections.Generic;
using HCWebService.Controllers;

namespace HCWebApp.Models
{
	public class HomeModel
    {
        public List<Quote> Quotes { get; set; }
        public List<Article> Articles { get; set; }
    }
}
