using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Formatting.Log
{
    public interface IDataTypeLogConverter
    {
        string[] ConverterStringToArrayLog(string logData);
        List<ItaasSolution.Api.Domain.Entities.Log> ConverterArrayToListObjectLog(string[] logArray);
        Task<string> ConverterFileUrlToFileStringLogAsync(string url);
    }
}
