using ItaasSolution.Api.Application.UseCases.Log.Converter;
using ItaasSolution.Api.Communication.Requests;
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
        /// <param name="request.IdLog">Is the id of an log of the database in that the data must be gotten and converted in format solicited.</param>
        /// <param name="request.UrlLog">Is the url with an file of an log in that the content the file must be converted in format solicited.</param>
        /// <returns>datas of play registered.</returns>
        [HttpPost("/api/log/converter")]
        public async Task<IActionResult> Converter([FromServices] IConverterLogUseCase usecase, [FromBody] RequestConverterLogJson request)
        {
            var response = await usecase.ExecuteAsync(request);
            return Created(string.Empty, response);
        }
    }
}
