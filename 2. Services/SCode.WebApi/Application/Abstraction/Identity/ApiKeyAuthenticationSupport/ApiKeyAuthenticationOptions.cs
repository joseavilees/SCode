using Microsoft.AspNetCore.Authentication;

namespace SCode.WebApi.Application.Abstraction.Identity.ApiKeyAuthenticationSupport
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}