using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Data;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;

namespace TechMoveGLMS.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;

        // constructor
        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        // get all clients from database
        public async Task<List<Client>> GetAllClientsAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            return clients;
        }

        // get a client by their id
        public async Task<Client> GetClientByIdAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            return client;
        }

        // create a new client
        public async Task CreateClientAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _context.Add(client);
            await _context.SaveChangesAsync();
        }

        // update client information
        public async Task UpdateClientAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            try
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // this happened before, not sure why
                throw ex;
            }
        }

        // delete a client by id
        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        // check if a client exists
        public async Task<bool> ClientExistsAsync(int id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }
    }
}
