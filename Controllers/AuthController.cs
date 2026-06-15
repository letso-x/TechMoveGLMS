using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TechMoveGLMS.Services;

namespace TechMoveGLMS.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenService _tokenService;

        public AuthController(IHttpClientFactory httpClientFactory, TokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient("GLMSAPI");
            var response = await client.PostAsJsonAsync("api/auth/login", new { username, password });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (string.IsNullOrWhiteSpace(result?.Token))
            {
                ModelState.AddModelError(string.Empty, "Login failed.");
                return View();
            }

            _tokenService.StoreToken(result.Token);
            return RedirectToAction("Index", "Home");
        }

        private sealed class LoginResponse
        {
            public string? Token { get; set; }
        }
    }
}
