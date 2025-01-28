using System;
using System.Collections.Generic;
using System.Linq;

namespace ItaasSolution.Api.Application.Services.Log.Converter
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
            List<Domain.Entities.Log> logs = new List<Domain.Entities.Log>();

            foreach (var logLine in logArray)
            {
                var parts = logLine.Split('|');
                var methodAndPath = parts[3].Split(' ');

                var log = new Domain.Entities.Log
                (
                    methodAndPath[0].Trim('"'),
                    int.Parse(parts[1]),
                    methodAndPath[1],
                    decimal.Parse(parts[4]),
                    int.Parse(parts[0]),
                    parts[2],
                    DateTime.UtcNow
                );

                logs.Add(log);
            }

            return logs;
        }
    }
}
