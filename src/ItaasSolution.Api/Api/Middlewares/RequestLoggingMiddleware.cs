using ItaasSolution.Api.Application.UseCases.Log.Register;
using ItaasSolution.Api.Communication.Requests.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream;

            // Execute the next middleware in the pipeline
            await _next(context);

            stopwatch.Stop();

            // Capture the request data after the response has been generated
            var httpMethod = context.Request.Method;
            var statusCode = context.Response.StatusCode;
            var uriPath = "/" + context.GetRouteData()?.Values["controller"]?.ToString();
            var timeTaken = stopwatch.ElapsedMilliseconds;
            var responseSize = (int)responseBodyStream.Length;
            var cacheStatus = "MISS";

            // Reset the response body stream position and read the response body text
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(responseBodyText))
            {
                // Gets the data of the cache status
                var responseBodyJson = JObject.Parse(responseBodyText);
                cacheStatus = !string.IsNullOrWhiteSpace(responseBodyJson["cacheStatus"]?.ToString()) ? responseBodyJson["cacheStatus"]?.ToString() : cacheStatus;

                // Write the responseData back to the response body stream
                var responseData = responseBodyJson["data"];
                if (responseData != null)
                {
                    responseBodyStream.SetLength(0); // Clear the stream
                    var newResponseBodyText = responseData.ToString();
                    await responseBodyStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(newResponseBodyText));
                }
            }

            // Sets the Cache Status
            context.Response.Headers.Add("X-Cache-Status", cacheStatus);

            // Resolve the scoped service within the InvokeAsync method
            var registerLogUseCase = context.RequestServices.GetRequiredService<IRegisterLogUseCase>();

            // Register the log of the request
            var request = new RequestLogJson()
            { 
                HtttpMethod = httpMethod,
                StatusCode = statusCode,
                UriPath = uriPath,
                TimeTaken = timeTaken,
                ResponseSize = responseSize,
                CacheStatus = cacheStatus,
            };
            await registerLogUseCase.ExecuteAsync(request);

            // Reset the response body stream position and copy it to the original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }
}
