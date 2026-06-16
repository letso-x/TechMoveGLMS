using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Test.Integration
{
    public class ContractsApiIntegrationTests : IClassFixture<GlmsApiFactory>
    {
        private readonly GlmsApiFactory _factory;

        public ContractsApiIntegrationTests(GlmsApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetContracts_ReturnsOk()
        {
            var client = await CreateAuthorizedClientAsync();

            var response = await client.GetAsync("/api/contracts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>(TestJsonOptions.Value);
            Assert.NotNull(contracts);
        }

        [Fact]
        public async Task GetActiveContracts_ReturnsOkAndOnlyActiveContracts()
        {
            var client = await CreateAuthorizedClientAsync();

            var response = await client.GetAsync("/api/contracts?status=Active");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>(TestJsonOptions.Value);
            Assert.NotNull(contracts);
            Assert.All(contracts, contract => Assert.Equal(ContractStatus.Active, contract.Status));
        }

        [Fact]
        public async Task PostContract_CreatesContractAndReturnsCreated()
        {
            var client = await CreateAuthorizedClientAsync();
            var clientId = await TestDataSeeder.EnsureClientAsync(client);

            var response = await client.PostAsJsonAsync("/api/contracts", CreateContract(clientId));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PatchContractStatus_UpdatesStatusAndReturnsOk()
        {
            var client = await CreateAuthorizedClientAsync();
            var clientId = await TestDataSeeder.EnsureClientAsync(client);
            var createResponse = await client.PostAsJsonAsync("/api/contracts", CreateContract(clientId));
            createResponse.EnsureSuccessStatusCode();
            var contract = await createResponse.Content.ReadFromJsonAsync<Contract>(TestJsonOptions.Value);

            var response = await client.PatchAsJsonAsync($"/api/contracts/{contract!.Id}/status", new
            {
                status = "Active"
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            var client = _factory.CreateClient();
            var token = await AuthHelper.GetBearerTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private static Contract CreateContract(int clientId)
        {
            return new Contract
            {
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddMonths(1),
                Status = ContractStatus.Draft,
                ServiceLevel = "Integration Test",
                ClientId = clientId
            };
        }
    }
}
