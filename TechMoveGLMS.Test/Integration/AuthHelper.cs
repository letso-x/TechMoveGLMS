using System.Net.Http.Json;

namespace TechMoveGLMS.Test.Integration
{
    public static class AuthHelper
    {
        public static async Task<string> GetBearerTokenAsync(HttpClient client)
        {
            var response = await client.PostAsJsonAsync("/api/auth/login", new
            {
                username = "admin",
                password = "password123"
            });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token ?? throw new InvalidOperationException("JWT token was not returned.");
        }

        private sealed class LoginResponse
        {
            public string? Token { get; set; }
        }
    }
}
