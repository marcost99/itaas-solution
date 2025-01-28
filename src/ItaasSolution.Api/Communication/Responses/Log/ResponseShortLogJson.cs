namespace ItaasSolution.Api.Communication.Responses.Log
{
    public class ResponseShortLogJson
    {
        public long Id { get; set; }
        public string HtttpMethod { get; set; }
        public int StatusCode { get; set; }
        public string UriPath { get; set; }
    }
}
