using System.Collections.Generic;

namespace ItaasSolution.Api.Communication.Responses.Log
{
    public class ResponseLogsJson
    {
        public List<ResponseShortLogJson> Logs { get; set; } = new List<ResponseShortLogJson>();
    }
}
