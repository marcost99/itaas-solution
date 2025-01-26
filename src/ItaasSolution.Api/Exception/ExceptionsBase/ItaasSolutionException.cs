using System.Collections.Generic;

namespace ItaasSolution.Api.Exception.ExceptionsBase
{
    public abstract class ItaasSolutionException : System.Exception
    {
        public abstract int StatusCode { get; }

        protected ItaasSolutionException(string message) : base(message) { }

        public abstract List<string> GetErrors();
    }
}
