using System.Collections.Generic;

namespace ItaasSolution.Api.Communication.Responses.FileLog
{
    public class ResponseConvertedFileLogsJson
    {
        public List<ResponseConvertedFileLogJson> FileLogs { get; set; } = new List<ResponseConvertedFileLogJson>();
    }
}
