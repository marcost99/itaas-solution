using ItaasSolution.Api.Communication.Enums;

namespace ItaasSolution.Api.Communication.Requests.FileLog
{
    public class RequestConverterFileLogJson
    {
        public FormatMadeAvailableLogConverted FormatMadeAvailableLogConverted { get; set; }
        public long IdLog { get; set; }
        public string UrlLog { get; set; }
    }
}
