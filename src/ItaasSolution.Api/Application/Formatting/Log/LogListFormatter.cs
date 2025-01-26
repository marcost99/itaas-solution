using System.Linq;

namespace ItaasSolution.Api.Application.Formatting.Log
{
    public class LogListFormatter : ILogListFormatter
    {
        public string[] ListFormatter(string logData)
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
    }
}
