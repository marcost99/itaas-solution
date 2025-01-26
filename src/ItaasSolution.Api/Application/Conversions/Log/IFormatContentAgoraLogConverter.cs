using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Conversions.Log
{
    public interface IFormatContentAgoraLogConverter
    {
        string ConverterListObjectToStringLogAgora(List<ItaasSolution.Api.Domain.Entities.Log> logs);
        Task<string> ConverterListObjectToUrlFileLogAgoraAsync(List<ItaasSolution.Api.Domain.Entities.Log> logs);
    }
}
