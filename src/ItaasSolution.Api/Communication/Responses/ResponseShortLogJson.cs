namespace ItaasSolution.Api.Communication.Responses
{
    public class ResponseShortLogJson
    {
        public long Id { get; set; }
        public string HtttpMethod { get; set; }
        public int StatusCode { get; set; }
        public string UriPath { get; set; }
    }
}
