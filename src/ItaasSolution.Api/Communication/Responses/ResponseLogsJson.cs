using System.Collections.Generic;

namespace ItaasSolution.Api.Communication.Responses
{
    public class ResponseLogsJson
    {
        public List<ResponseShortLogJson> Logs { get; set; } = new List<ResponseShortLogJson>();
    }
}
