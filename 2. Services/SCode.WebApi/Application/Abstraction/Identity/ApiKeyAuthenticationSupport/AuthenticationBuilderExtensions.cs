using System;
using Microsoft.AspNetCore.Authentication;

namespace SCode.WebApi.Application.Abstraction.Identity.ApiKeyAuthenticationSupport
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeyAuthentication(this AuthenticationBuilder authenticationBuilder,
            Action<ApiKeyAuthenticationOptions> options = default)
        {
            return authenticationBuilder
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
        }
    }
}