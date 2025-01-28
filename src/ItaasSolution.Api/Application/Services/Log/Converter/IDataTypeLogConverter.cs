using System.Collections.Generic;

namespace ItaasSolution.Api.Application.Services.Log.Converter
{
    public interface IDataTypeLogConverter
    {
        string[] ConverterStringToArrayLog(string logData);
        List<Domain.Entities.Log> ConverterArrayToListObjectLog(string[] logArray);
    }
}
