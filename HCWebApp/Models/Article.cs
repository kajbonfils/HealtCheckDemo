using System;

namespace HCWebApp.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string ArticleContent { get; set; }
        public DateTime PublishingDate { get; set; }
    }
}