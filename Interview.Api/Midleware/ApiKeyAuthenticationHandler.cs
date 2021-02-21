using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Interview.Application;
using Interview.Application.Core.Entitities;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Interview.Api.Midleware
{
    public class ApiKeyAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    { }

    public class ApiKeyAuthenticationHandler: AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IApplicationRepository<UserInfo> _applicationUserRepository;
        private readonly IApplicationSerializationService _applicationSerializationService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApplicationRepository<UserInfo> applicationUserRepository,
            IApplicationSerializationService applicationSerializationService)
            : base(options, logger, encoder, clock)
        {
            _applicationUserRepository = applicationUserRepository;
            _applicationSerializationService = applicationSerializationService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(ApplicationConstants.Authentication.AuthenticationHeaderName))
            {
                var errorMessage = ApplicationConstants.ErrorMessages.AutheticationHeaderNotFound;

                WriteAutheticationResponse(errorMessage);

                return Task.FromResult(AuthenticateResult.Fail(errorMessage));
            }

            var apiKey = Request.Headers[ApplicationConstants.Authentication.AuthenticationHeaderName].ToString();
            if(string.IsNullOrEmpty(apiKey))
            {
                var errorMessage = ApplicationConstants.ErrorMessages.AutheticationHeaderInvalidValue;

                WriteAutheticationResponse(errorMessage);

                return Task.FromResult(AuthenticateResult.Fail(errorMessage));
            }

            var userInfo = GetUserInfo(apiKey).GetAwaiter().GetResult();
            if (userInfo == null)
            {
                var errorMessage = string.Format(ApplicationConstants.ErrorMessages.AutheticationApiKeyNotFound, apiKey);

                WriteAutheticationResponse(errorMessage);

                return Task.FromResult(AuthenticateResult.Fail(errorMessage));
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString()) };

            var claimsIdentity = new ClaimsIdentity(claims, nameof(ApiKeyAuthenticationHandler));

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
           
        }

        private async Task<UserInfo> GetUserInfo(string apiKey)
        {
            return await _applicationUserRepository.SingleOrDefaultAsync(user => user.ApiKey == apiKey);
        }

        private void WriteAutheticationResponse(string errorMessage)
        {
            var customError = new ApplicationError(ApplicationConstants.ErrorCodes.AutheticationError, errorMessage);

            var serializedError = _applicationSerializationService.Serialize(customError);

            Context.Response.StatusCode  = (int)HttpStatusCode.Unauthorized;

            Context.Response.ContentType = ApplicationConstants.Authentication.AuthenticationResponseContentType;

            Context.Response.WriteAsync(serializedError).Wait();
        }
    }

}
