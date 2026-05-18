using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public class ContractService : IContractService
    {
        private readonly AppDbContext _context;

        // constructor that takes the database context
        public ContractService(AppDbContext context)
        {
            _context = context;
        }

        // gets all contracts, with optional filtering
        public async Task<List<Contract>> GetAllContractsAsync(DateTime? startDate = null, DateTime? endDate = null, ContractStatus? status = null)
        {
            var query = _context.Contracts.AsQueryable();

            // filter by start date if provided
            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate);
            }

            // filter by end date if provided
            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate);
            }

            // filter by status if provided
            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status);
            }

            // execute the query
            var result = await query.ToListAsync();
            return result;
        }

        // get a contract by id
        public async Task<Contract> GetContractByIdAsync(int id)
        {
            var contract = await _context.Contracts.FirstOrDefaultAsync(c => c.Id == id);
            return contract;
        }

        // create a new contract in the database
        public async Task CreateContractAsync(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            _context.Add(contract);
            // save the changes to database
            await _context.SaveChangesAsync();
        }

        // update an existing contract
        public async Task UpdateContractAsync(Contract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            try
            {
                _context.Update(contract);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                throw;
            }
        }

        // delete a contract by id
        public async Task DeleteContractAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
        }

        // check if contract exists
        public async Task<bool> ContractExistsAsync(int id)
        {
            var exists = await _context.Contracts.AnyAsync(c => c.Id == id);
            return exists;
        }
    }
}
