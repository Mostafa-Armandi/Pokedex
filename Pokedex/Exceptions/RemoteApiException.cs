using System;
using System.Net;

namespace Pokedex.Exceptions
{
    public class RemoteApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public RemoteApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}