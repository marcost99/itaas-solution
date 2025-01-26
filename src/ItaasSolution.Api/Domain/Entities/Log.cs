using System;

namespace ItaasSolution.Api.Domain.Entities
{
    public class Log
    {
        public long Id { get; set; }
        public string HtttpMethod { get; set; }
        public int StatusCode { get; set; }
        public string UriPath { get; set; }
        public decimal TimeTaken { get; set; }
        public int ResponseSize { get; set; }
        public string CacheStatus { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
