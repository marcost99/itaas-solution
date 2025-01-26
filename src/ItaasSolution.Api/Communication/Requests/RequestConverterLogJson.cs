using ItaasSolution.Api.Communication.Enums;

namespace ItaasSolution.Api.Communication.Requests
{
    public class RequestConverterLogJson
    {
        public FormatMadeAvailableLogConverted FormatMadeAvailableLogConverted { get; set; }
        public long IdLog { get; set; }
        public string UrlLog { get; set; }
    }
}
