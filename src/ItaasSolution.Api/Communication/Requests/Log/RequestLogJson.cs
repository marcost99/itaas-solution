namespace ItaasSolution.Api.Communication.Requests.Log
{
    public class RequestLogJson
    {
        public string HtttpMethod { get; set; }
        public int StatusCode { get; set; }
        public string UriPath { get; set; }
        public decimal TimeTaken { get; set; }
        public int ResponseSize { get; set; }
        public string CacheStatus { get; set; }
    }
}
