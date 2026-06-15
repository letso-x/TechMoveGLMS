using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.API.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceRequestsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            var serviceRequests = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .ToListAsync();

            return Ok(serviceRequests);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> CreateServiceRequest(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);
            if (contract == null || contract.Status != ContractStatus.Active)
            {
                return BadRequest("Service requests can only be created for an active contract.");
            }

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceRequest), new { id = serviceRequest.Id }, serviceRequest);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests
                .Include(sr => sr.Contract)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return Ok(serviceRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceRequest(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return BadRequest();
            }

            var existingServiceRequest = await _context.ServiceRequests.FindAsync(id);
            if (existingServiceRequest == null)
            {
                return NotFound();
            }

            existingServiceRequest.Description = serviceRequest.Description;
            existingServiceRequest.ContractId = serviceRequest.ContractId;
            existingServiceRequest.Cost = serviceRequest.Cost;
            existingServiceRequest.Status = serviceRequest.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
