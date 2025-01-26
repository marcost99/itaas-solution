using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Formatting.Log
{
    public class DataTypeLogConverter : IDataTypeLogConverter
    {
        public string[] ConverterStringToArrayLog(string logData)
        {
            var lines = logData.Split('\n');

            if (lines.Length > 1)
            {
                if (string.IsNullOrWhiteSpace(lines[lines.Length - 1]))
                {
                    var linesList = lines.ToList();
                    linesList.RemoveAt(linesList.Count - 1);
                    lines = linesList.ToArray();
                }
            }

            return lines;
        }

        public List<Domain.Entities.Log> ConverterArrayToListObjectLog(string[] logArray)
        {
            List<ItaasSolution.Api.Domain.Entities.Log> logs = new List<ItaasSolution.Api.Domain.Entities.Log>();

            foreach (var logLine in logArray)
            {
                var parts = logLine.Split('|');
                var methodAndPath = parts[3].Split(' ');

                var log = new ItaasSolution.Api.Domain.Entities.Log
                {
                    ResponseSize = long.Parse(parts[0]),
                    StatusCode = long.Parse(parts[1]),
                    CacheStatus = parts[2],
                    HtttpMethod = methodAndPath[0].Trim('"'),
                    UriPath = methodAndPath[1],
                    TimeTaken = decimal.Parse(parts[4])
                };

                logs.Add(log);
            }

            return logs;
        }

        // This method get the content the an file the text of an URL
        public async Task<string> ConverterFileUrlToFileStringLogAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}
