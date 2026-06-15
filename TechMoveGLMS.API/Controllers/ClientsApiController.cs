using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.API.Data;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/clients")]
    public class ClientsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients
                .Include(c => c.Contracts)
                .ToListAsync();

            return Ok(clients);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Created($"/api/clients/{client.Id}", client);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.PhoneNumber = client.PhoneNumber;
            existingClient.Region = client.Region;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
