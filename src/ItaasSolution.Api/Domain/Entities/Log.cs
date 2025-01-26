using System;

namespace ItaasSolution.Api.Domain.Entities
{
    public class Log
    {
        public long Id { get; set; }
        private string _htttpMethod { get; set; }
        private int _statusCode { get; set; }
        private string _uriPath { get; set; }
        private decimal _timeTaken { get; set; }
        private int _responseSize { get; set; }
        private string _cacheStatus { get; set; }
        private DateTime _dateCreation { get; set; }

        public string HtttpMethod { get => _htttpMethod; set => _htttpMethod = value; }
        public int StatusCode { get => _statusCode; set => _statusCode = value; }
        public string UriPath { get => _uriPath; set => _uriPath = value; }
        public decimal TimeTaken { get => _timeTaken; set => _timeTaken = value; }
        public int ResponseSize { get => _responseSize; set => _responseSize = value; }
        public string CacheStatus { get => _cacheStatus; set => _cacheStatus = value; }
        public DateTime DateCreation { get => _dateCreation; set => _dateCreation = value; }

        public Log(string htttpMethod, int statusCode, string uriPath, decimal timeTaken, int responseSize, string cacheStatus, DateTime dateCreation)
        {
            this._htttpMethod = htttpMethod;
            this._statusCode = statusCode;
            this._uriPath = uriPath;
            this._timeTaken = timeTaken;
            this._responseSize = responseSize;
            this._cacheStatus = cacheStatus;
            this._dateCreation = dateCreation;
        }
    }
}
