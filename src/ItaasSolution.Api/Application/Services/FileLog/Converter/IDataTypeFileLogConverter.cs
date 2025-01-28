using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Services.FileLog.Converter
{
    public interface IDataTypeFileLogConverter
    {
        string ConverterListObjectToStringFileLog(List<Domain.Entities.Log> logs);
        Task<string> ConverterListObjectToUrlFileLogAsync(List<Domain.Entities.Log> logs, long idFileLog);
        Task<string> ConverterFileUrlToStringFileLogAsync(string url);
    }
}
