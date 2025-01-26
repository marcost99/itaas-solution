using System.Collections.Generic;
using System.Net;

namespace ItaasSolution.Api.Exception.ExceptionsBase
{
    public class NotFoundException : ItaasSolutionException
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message) { }

        public override List<string> GetErrors()
        {
            return new List<string>() { Message };
        }
    }
}
