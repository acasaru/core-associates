using Interview.Application;
using Interview.Application.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Interview.Api.Midleware
{
    public class ApiMidlewareDefinitions
    {
        public static Func<ActionContext, IActionResult> InvalidModelStateResponseFactory = (context) =>
        {
            var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
            var result = new ApplicationError(
                ApplicationConstants.ErrorCodes.InputValidationError,
                ApplicationConstants.ErrorMessages.InputValidationErrors,
                errors);
            
            return new BadRequestObjectResult(result);
        };
    }
}
