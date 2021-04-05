using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;
using SCode.Shared.Requests.Account;

namespace SCode.Client.Teacher.ConsoleApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly AppSettings _appSettings;

        private readonly string _accountEndpoint;

        public AccountService(ILogger<AccountService> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;

            _accountEndpoint = new Url(appSettings.SCodeApiUrl)
                .AppendPathSegment("api")
                .AppendPathSegment("Account");
        }

        public async Task<bool> Login()
        {
            try
            {
                var url = _accountEndpoint
                    .AppendPathSegment("ValidateApiKey");

                var request =
                    new ValidateApiKeyRequest(_appSettings.ApiKey);

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();

                var result = await response
                    .Content
                    .ReadFromJsonAsync<bool>();

                if (result)
                    _logger.LogDebug("API Key válida");
     
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "No fue posible validar la API Key");

                return false;
            }
        }
    }

    public interface IAccountService
    {
        Task<bool> Login();
    }
}