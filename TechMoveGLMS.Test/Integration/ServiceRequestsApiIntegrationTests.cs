using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Test.Integration
{
    public class ServiceRequestsApiIntegrationTests : IClassFixture<GlmsApiFactory>
    {
        private readonly GlmsApiFactory _factory;

        public ServiceRequestsApiIntegrationTests(GlmsApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetServiceRequests_ReturnsOk()
        {
            var client = await CreateAuthorizedClientAsync();

            var response = await client.GetAsync("/api/servicerequests");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var serviceRequests = await response.Content.ReadFromJsonAsync<List<ServiceRequest>>(TestJsonOptions.Value);
            Assert.NotNull(serviceRequests);
        }

        [Fact]
        public async Task PostServiceRequestWithInactiveContract_ReturnsBadRequest()
        {
            var client = await CreateAuthorizedClientAsync();
            var contract = await CreateContractAsync(client, ContractStatus.Draft);

            var response = await client.PostAsJsonAsync("/api/servicerequests", CreateServiceRequest(contract.Id));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostServiceRequestWithActiveContract_ReturnsCreated()
        {
            var client = await CreateAuthorizedClientAsync();
            var contract = await CreateContractAsync(client, ContractStatus.Draft);
            var patchResponse = await client.PatchAsJsonAsync($"/api/contracts/{contract.Id}/status", new
            {
                status = "Active"
            });
            patchResponse.EnsureSuccessStatusCode();

            var response = await client.PostAsJsonAsync("/api/servicerequests", CreateServiceRequest(contract.Id));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            var client = _factory.CreateClient();
            var token = await AuthHelper.GetBearerTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private async Task<Contract> CreateContractAsync(HttpClient client, ContractStatus status)
        {
            var clientId = await TestDataSeeder.EnsureClientAsync(client);
            var response = await client.PostAsJsonAsync("/api/contracts", new Contract
            {
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddMonths(1),
                Status = status,
                ServiceLevel = "Integration Test",
                ClientId = clientId
            });

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Contract>(TestJsonOptions.Value)
                ?? throw new InvalidOperationException("Contract creation failed.");
        }

        private static ServiceRequest CreateServiceRequest(int contractId)
        {
            return new ServiceRequest
            {
                Description = "Integration test service request",
                ContractId = contractId,
                Cost = 123.45m,
                Status = ServiceRequestStatus.Draft
            };
        }
    }
}
