using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SCode.WebApi.Application.Services;

namespace SCode.WebApi.Application.Abstraction.Identity.ApiKeyAuthenticationSupport
{
    /// <summary>
    /// ApiKeyAuthenticationHandler
    ///
    /// https://josef.codes/asp-net-core-protect-your-api-with-api-keys/
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly ICustomAuthenticationService _authenticationService;
        
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            ICustomAuthenticationService authenticationService) : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authResult = _authenticationService.Authenticate(providedApiKey);
            if (authResult)
            {
                var identity = new ClaimsIdentity(Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> {identity};
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Bad API Key"));
        }
    }
}