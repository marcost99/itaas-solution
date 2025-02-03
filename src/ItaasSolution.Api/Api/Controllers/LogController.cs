using ItaasSolution.Api.Application.UseCases.Log.GetAll;
using ItaasSolution.Api.Application.UseCases.Log.GetById;
using ItaasSolution.Api.Application.UseCases.Log.Register;
using ItaasSolution.Api.Communication.Requests.Log;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Communication.Responses.Log;
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
        /// registers a new log [6-Salvar Logs].
        /// </summary>
        /// <param name="request">model of request.</param>
        /// <param name="request.HtttpMethod">Example: GET</param>
        /// <param name="request.StatusCode">Example: 200</param>
        /// <param name="request.UriPath">Example: /robots.txt</param>
        /// <param name="request.TimeTaken">Example: 100.2</param>
        /// <param name="request.ResponseSize">Example: 312</param>
        /// <param name="request.CacheStatus">Example: HIT</param>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterLogJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromServices] IRegisterLogUseCase usecase, [FromBody] RequestLogJson request)
        {
            var response = await usecase.ExecuteAsync(request);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// gets all logs [2-Buscar Logs Salvos].
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseLogsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllAsync([FromServices] IGetAllLogUseCase useCase)
        {
            var (cacheStatus, data) = await useCase.ExecuteAsync();

            if (data.Logs.Count > 0)
                return Ok(new { CacheStatus = cacheStatus, Data = data });

            return NoContent();
        }

        /// <summary>
        /// gets log by id [4-Buscar Logs Salvos por Identicador].
        /// </summary>
        /// <param name="id">Id of log. Exemple: 1.</param>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseLogJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromServices] IGetByIdLogUseCase useCase, [FromRoute] long id)
        {
            var response = await useCase.ExecuteAsync(id);

            return Ok(response);
        }
    }
}
