namespace SCode.Shared.Requests.Account
{
    public class ValidateApiKeyRequest
    {
        public string ApiKey { get; set; }

        public ValidateApiKeyRequest(string apiKey)
        {
            ApiKey = apiKey;
        }
    }
}