using ItaasSolution.Api.Application.UseCases.Log.Converter;
using ItaasSolution.Api.Application.UseCases.Log.Register;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        /// <summary>
        /// converter log in format solicited.
        /// </summary>
        /// <param name="request">model of request.</param>
        /// <param name="request.FormatMadeAvailableLogConverted">Is the format made available log converted. Chooses entry the seguints options (0: url of the file of the log; 1: content the log)</param>
        /// <param name="request.IdLog">If the converter is fur id of the log must be filled this field. Is the id of an log of the database in that the data must be gotten and converted in format solicited. Example: 1</param>
        /// <param name="request.UrlLog">If the converter is fur url of the log must be filled this field. Is the url with an file of an log in that the content the file must be converted in format solicited. Example: https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt</param>
        /// <returns>datas of play registered.</returns>
        [HttpPost("/api/log/converter")]
        [ProducesResponseType(typeof(ResponseConverterLogJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConverterAsync([FromServices] IConverterLogUseCase usecase, [FromBody] RequestConverterLogJson request)
        {
            var response = await usecase.ExecuteAsync(request);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// register an new log.
        /// </summary>
        /// <param name="request">model of request.</param>
        /// <param name="request.HtttpMethod">Examplo: GET</param>
        /// <param name="request.StatusCode">Examplo: 200</param>
        /// <param name="request.UriPath">Examplo: /robots.txt</param>
        /// <param name="request.TimeTaken">Examplo: 100.2</param>
        /// <param name="request.ResponseSize">Examplo: 312</param>
        /// <param name="request.CacheStatus">Examplo: HIT</param>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterLogJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromServices] IRegisterLogUseCase usecase, [FromBody] RequestLogJson request)
        {
            var response = await usecase.ExecuteAsync(request);
            return Created(string.Empty, response);
        }
    }
}
