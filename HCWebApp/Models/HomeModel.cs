using System.Collections.Generic;
using HCWebService.Controllers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HCWebApp.Models
{
    public class HomeModel
    {
        public List<Quote> Quotes { get; set; }
        public List<Article> Articles { get; set; }
        public string HealthJson { get; internal set; }
        public HealthReport Health { get; internal set; }
    }
}
