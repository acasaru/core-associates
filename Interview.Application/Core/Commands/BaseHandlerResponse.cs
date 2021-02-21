using Interview.Application.Core.Exceptions;
using System.Collections.Generic;
using System.Net;


namespace Interview.Application.Core.Commands
{
    public class BaseHandlerResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApplicationError Error { get; set; }

        public BaseHandlerResponse(HttpStatusCode status)
        {
            StatusCode = status;
        }
        public BaseHandlerResponse(HttpStatusCode status, ApplicationError error)
        {
            StatusCode = status;
            Error = error;
        }
        public BaseHandlerResponse(HttpStatusCode status, string errorCode, string errorMessage, List<string> additionalInfo)
        {
            StatusCode = status;
            Error = new ApplicationError(errorCode, errorMessage, additionalInfo);
        }

        public bool HasError => Error != null;

    }
}
