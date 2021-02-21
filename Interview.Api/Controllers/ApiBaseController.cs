using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Interview.Api.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        
        public ApiBaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult<T> ProcessResult<T>(BaseHandlerResponseWithContent<T> response) where T : class
        {
            var error = response.Error;

            T content = response.HasError ? null : response.Content;

            return ProcessResult(content, error, response.StatusCode);
        }

        protected ActionResult ProcessResult(BaseHandlerResponse response)
        {
            return ProcessResult(null as object, response.Error, response.StatusCode);
        }


        private ObjectResult ProcessResult<T>(T content, ApplicationError error, HttpStatusCode statusCode) where T : class
        {
            //workaround for HttpNoContentOutputFormatter
            if (statusCode == HttpStatusCode.OK && content == null)
            {
                return new OkObjectResult(string.Empty);
            }

            return error == null ?
                StatusCode((int)statusCode, content) :
                StatusCode((int)statusCode, error);
        }
    }
}
