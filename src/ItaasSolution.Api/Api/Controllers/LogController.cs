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
        [HttpPost("/api/log/converter")]
        public async Task<IActionResult> Converter([FromServices] IConverterLogUseCase usecase, [FromBody] RequestConverterLogJson request)
        {
            var response = await usecase.Execute(request);
            return Created(string.Empty, response);
        }
    }
}
