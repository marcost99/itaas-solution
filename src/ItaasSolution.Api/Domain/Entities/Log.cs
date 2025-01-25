namespace ItaasSolution.Api.Domain.Entities
{
    public class Log
    {
        public long Id { get; set; }
        public string HtttpMethod { get; set; }
        public long StatusCode { get; set; }
        public string UriPath { get; set; }
        public decimal TimeTaken { get; set; }
        public long ResponseSize { get; set; }
        public string CacheStatus { get; set; }
    }
}
