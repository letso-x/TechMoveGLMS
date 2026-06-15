using System.Net;
using System.Net.Http.Json;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public class ContractService : IContractService
    {
        private readonly ApiService _apiService;

        public ContractService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<Contract>> GetAllContractsAsync(DateTime? startDate = null, DateTime? endDate = null, ContractStatus? status = null)
        {
            var client = _apiService.GetAuthorizedClient();
            var url = status.HasValue ? $"api/contracts?status={status.Value}" : "api/contracts";
            var contracts = await client.GetFromJsonAsync<List<Contract>>(url) ?? new List<Contract>();

            if (startDate.HasValue)
            {
                contracts = contracts.Where(c => c.StartDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                contracts = contracts.Where(c => c.EndDate <= endDate.Value).ToList();
            }

            return contracts;
        }

        public async Task<Contract> GetContractByIdAsync(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.GetAsync($"api/contracts/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Contract>();
        }

        public async Task CreateContractAsync(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var client = _apiService.GetAuthorizedClient();
            var response = await client.PostAsJsonAsync("api/contracts", contract);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var client = _apiService.GetAuthorizedClient();
            var response = await client.PutAsJsonAsync($"api/contracts/{contract.Id}", contract);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteContractAsync(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.DeleteAsync($"api/contracts/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ContractExistsAsync(int id)
        {
            var client = _apiService.GetAuthorizedClient();
            var response = await client.GetAsync($"api/contracts/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
