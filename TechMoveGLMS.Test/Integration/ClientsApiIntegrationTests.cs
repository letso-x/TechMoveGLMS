using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Test.Integration
{
    public class ClientsApiIntegrationTests : IClassFixture<GlmsApiFactory>
    {
        private readonly GlmsApiFactory _factory;

        public ClientsApiIntegrationTests(GlmsApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetClients_ReturnsOk()
        {
            var client = await CreateAuthorizedClientAsync();

            var response = await client.GetAsync("/api/clients");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var clients = await response.Content.ReadFromJsonAsync<List<Client>>(TestJsonOptions.Value);
            Assert.NotNull(clients);
        }

        [Fact]
        public async Task PostClient_CreatesClientAndReturnsCreated()
        {
            var client = await CreateAuthorizedClientAsync();

            var response = await client.PostAsJsonAsync("/api/clients", new Client
            {
                Name = "Integration Test Client",
                Email = $"integration.{Guid.NewGuid():N}@example.com",
                PhoneNumber = "0000000000",
                Region = "Test",
                Contracts = new List<Contract>()
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            var client = _factory.CreateClient();
            var token = await AuthHelper.GetBearerTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
