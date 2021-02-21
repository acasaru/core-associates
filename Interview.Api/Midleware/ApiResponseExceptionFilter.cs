using Interview.Application;
using Interview.Application.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Interview.Api.Midleware
{
    public class ApiResponseExceptionFilter: IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ApplicationErrorException appException)
            {
                context.Result = new ObjectResult(appException.Error)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };

                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is OperationCanceledException)
            {
                var opCancelledError = new ApplicationError(ApplicationConstants.ErrorCodes.OperationCancelledError,
                    ApplicationConstants.ErrorMessages.OperationCancelledError);

                context.Result = new ObjectResult(opCancelledError);
                
                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is Exception)
            {
                var error = new ApplicationError(ApplicationConstants.ErrorCodes.UnknownError,
                   ApplicationConstants.ErrorMessages.UnknownError);

                context.Result = new ObjectResult(error)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                context.ExceptionHandled = true;
            }

        }
    }
}
