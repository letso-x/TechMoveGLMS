using System.Net;
using System.Net.Http.Json;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public class ClientService : IClientService
    {
        private readonly ApiService _apiService;

        public ClientService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            var client = _apiService.GetAuthorizedClient();
            return await client.GetFromJsonAsync<List<Client>>("api/clients") ?? new List<Client>();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.GetAsync($"api/clients/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Client>();
        }

        public async Task CreateClientAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var apiClient = _apiService.GetAuthorizedClient();
            var response = await apiClient.PostAsJsonAsync("api/clients", client);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateClientAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var apiClient = _apiService.GetAuthorizedClient();
            var response = await apiClient.PutAsJsonAsync($"api/clients/{client.Id}", client);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.DeleteAsync($"api/clients/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ClientExistsAsync(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.GetAsync($"api/clients/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
