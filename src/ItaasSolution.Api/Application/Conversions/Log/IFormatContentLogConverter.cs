using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Conversions.Log
{
    public interface IFormatContentLogConverter
    {
        string ConverterListObjectToStringLog(List<ItaasSolution.Api.Domain.Entities.Log> logs);
        Task<string> ConverterListObjectToUrlFileLogAsync(List<ItaasSolution.Api.Domain.Entities.Log> logs, long idFileLog);
    }
}
