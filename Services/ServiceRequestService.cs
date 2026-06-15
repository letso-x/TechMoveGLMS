using System.Net;
using System.Net.Http.Json;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly ApiService _apiService;

        public ServiceRequestService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<ServiceRequest>> GetAll()
        {
            var client = _apiService.GetAuthorizedClient();
            return await client.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests") ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest> GetById(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.GetAsync($"api/servicerequests/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceRequest>();
        }

        public async Task CreateRequest(ServiceRequest request)
        {
            if (request == null)
            {
                throw new Exception("Request cannot be null");
            }

            var client = _apiService.GetAuthorizedClient();
            var response = await client.PostAsJsonAsync("api/servicerequests", request);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(message) ? "Could not create service request." : message);
            }
        }

        public async Task Update(int id, ServiceRequest request)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.PutAsJsonAsync($"api/servicerequests/{id}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.DeleteAsync($"api/servicerequests/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
