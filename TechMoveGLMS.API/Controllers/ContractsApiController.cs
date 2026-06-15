using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.API.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/contracts")]
    public class ContractsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContractsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts([FromQuery] string? status)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!Enum.TryParse<ContractStatus>(status, true, out var parsedStatus))
                {
                    return BadRequest("Invalid contract status.");
                }

                query = query.Where(c => c.Status == parsedStatus);
            }

            return Ok(await query.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Contract>> CreateContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return Created($"/api/contracts/{contract.Id}", contract);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(int id, Contract contract)
        {
            if (id != contract.Id)
            {
                return BadRequest();
            }

            var existingContract = await _context.Contracts.FindAsync(id);
            if (existingContract == null)
            {
                return NotFound();
            }

            existingContract.StartDate = contract.StartDate;
            existingContract.EndDate = contract.EndDate;
            existingContract.Status = contract.Status;
            existingContract.SignedAgreement = contract.SignedAgreement;
            existingContract.ServiceLevel = contract.ServiceLevel;
            existingContract.ClientId = contract.ClientId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<Contract>> UpdateStatus(int id, [FromBody] UpdateContractStatusRequest request)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            contract.Status = request.Status;
            await _context.SaveChangesAsync();

            return Ok(contract);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public record UpdateContractStatusRequest(ContractStatus Status);
}
