﻿using ItaasSolution.Api.Application.UseCases.FileLog.Converter;
using ItaasSolution.Api.Communication.Requests.FileLog;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Communication.Responses.FileLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileLogController : ControllerBase
    {
        /// <summary>
        /// converter log in format solicited.
        /// </summary>
        /// <param name="request">model of request.</param>
        /// <param name="request.FormatMadeAvailableLogConverted">Is the format made available log converted. Chooses entry the seguints options (0: url of the file of the log; 1: content the log)</param>
        /// <param name="request.IdLog">If the converter is fur id of the log must be filled this field. Is the id of an log of the database in that the data must be gotten and converted in format solicited. Example: 1</param>
        /// <param name="request.UrlLog">If the converter is fur url of the log must be filled this field. Is the url with an file of an log in that the content the file must be converted in format solicited. Example: https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt</param>
        /// <returns>datas of play registered.</returns>
        [HttpPost("/api/filelog/converter")]
        [ProducesResponseType(typeof(ResponseConverterFileLogJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FileConverterAsync([FromServices] IConverterFileLogUseCase usecase, [FromBody] RequestConverterFileLogJson request)
        {
            var response = await usecase.ExecuteAsync(request);
            return Created(string.Empty, response);
        }
    }
}
