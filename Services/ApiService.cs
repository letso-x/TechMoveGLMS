using System.Net.Http.Headers;

namespace TechMoveGLMS.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenService _tokenService;

        public ApiService(IHttpClientFactory httpClientFactory, TokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        public HttpClient GetAuthorizedClient()
        {
            var client = _httpClientFactory.CreateClient("GLMSAPI");
            var token = _tokenService.GetToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
