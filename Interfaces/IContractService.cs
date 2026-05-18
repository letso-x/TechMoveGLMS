using TechMoveGLMS.Models;

namespace TechMoveGLMS.Interfaces
{
    public interface IContractService
    {

        Task<List<Contract>> GetAllContractsAsync(DateTime? startDate = null, DateTime? endDate = null, ContractStatus? status = null);

        
        Task<Contract> GetContractByIdAsync(int id);

        
        Task CreateContractAsync(Contract contract);

        
        Task UpdateContractAsync(Contract contract);

        
        Task DeleteContractAsync(int id);

        
        Task<bool> ContractExistsAsync(int id);
    }
}
