using System.Net.Http.Json;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Test.Integration
{
    public static class TestDataSeeder
    {
        public static async Task<int> EnsureClientAsync(HttpClient client)
        {
            var clients = await client.GetFromJsonAsync<List<Client>>("/api/clients", TestJsonOptions.Value) ?? new List<Client>();
            var existingClient = clients.FirstOrDefault();
            if (existingClient != null)
            {
                return existingClient.Id;
            }

            var response = await client.PostAsJsonAsync("/api/clients", new Client
            {
                Name = "Integration Test Client",
                Email = "integration.client@example.com",
                PhoneNumber = "0000000000",
                Region = "Test",
                Contracts = new List<Contract>()
            });

            response.EnsureSuccessStatusCode();
            var createdClient = await response.Content.ReadFromJsonAsync<Client>(TestJsonOptions.Value);
            return createdClient?.Id ?? throw new InvalidOperationException("Client seed failed.");
        }
    }
}
