using Interview.Application;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Interview.Api.Midleware
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IApplicationSerializationService _applicationSerializationService;

        public RolesAuthorizationHandler(
            IHttpContextAccessor httpContextAccesor,
            IApplicationSerializationService applicationSerializationService) : base()
        {
            _applicationSerializationService = applicationSerializationService;
            _httpContextAccesor = httpContextAccesor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();

                return Task.CompletedTask;
            }
            
            var validRole = false;
            if (requirement.AllowedRoles == null ||
                requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                var roles = requirement.AllowedRoles;

                validRole = roles.Contains(userRole);
            }

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                var errorMesage = string.Format(ApplicationConstants.ErrorMessages.AuthorizationErrorUserNotInRole,
                    string.Join(",", requirement.AllowedRoles?? new List<string>()));

                context.Fail();

                WriteAuthorizationResponse(errorMesage);
            }
            return Task.CompletedTask;
        }

        private void WriteAuthorizationResponse(string errorMessage)
        {
            var customError = new ApplicationError(ApplicationConstants.ErrorCodes.AuthorizationError, errorMessage);

            var serializedError = _applicationSerializationService.Serialize(customError);

            _httpContextAccesor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            _httpContextAccesor.HttpContext.Response.ContentType = ApplicationConstants.Authentication.AuthenticationResponseContentType;

            _httpContextAccesor.HttpContext.Response.WriteAsync(serializedError).Wait();
        }
    }
}  

