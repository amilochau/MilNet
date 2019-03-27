using System;

namespace MilNet.Services.Models
{
    public class LogOptions
    {
        public Uri ElasticSearchUri { get; set; } = new Uri("http://localhost:9200");
    }
}
