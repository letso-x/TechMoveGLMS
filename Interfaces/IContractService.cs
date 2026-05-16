using TechMoveGLMS.Models;

namespace TechMoveGLMS.Interfaces
{
    public interface IContractService
    {
        // get all contracts or filtered ones
        Task<List<Contract>> GetAllContractsAsync(DateTime? startDate = null, DateTime? endDate = null, ContractStatus? status = null);

        // get a single contract by id
        Task<Contract> GetContractByIdAsync(int id);

        // create new contract
        Task CreateContractAsync(Contract contract);

        // update existing contract
        Task UpdateContractAsync(Contract contract);

        // delete a contract
        Task DeleteContractAsync(int id);

        // check if contract exists
        Task<bool> ContractExistsAsync(int id);
    }
}
