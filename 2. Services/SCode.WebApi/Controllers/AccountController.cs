using Microsoft.AspNetCore.Mvc;
using SCode.Shared.Requests.Account;
using SCode.WebApi.Application.Services;

namespace SCode.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ICustomAuthenticationService _authenticationService;

        public AccountController(ICustomAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public IActionResult ValidateApiKey(ValidateApiKeyRequest request)
        {
            return Ok(_authenticationService
                .Authenticate(request.ApiKey));
        }
    }
}