using System;

namespace SCode.WebApi.Application.Services
{
    public class CustomAuthenticationService : ICustomAuthenticationService
    {
        private readonly string _apiKey;

        public CustomAuthenticationService()
        {
            _apiKey = Environment.GetEnvironmentVariable("API_KEY");
        }


        public bool Authenticate(string apiKey)
        {
            return apiKey.Equals(_apiKey, StringComparison.CurrentCulture);
        }
    }
    
    public interface ICustomAuthenticationService
    {
        bool Authenticate(string apiKey);
    }
}