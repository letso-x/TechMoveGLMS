using TechMoveGLMS.Models;

namespace TechMoveGLMS.Interfaces
{
    public interface IClientService
    {
        // get all clients
        Task<List<Client>> GetAllClientsAsync();

        // get one client by id
        Task<Client> GetClientByIdAsync(int id);

        // add new client
        Task CreateClientAsync(Client client);

        // update client info
        Task UpdateClientAsync(Client client);

        // remove a client
        Task DeleteClientAsync(int id);

        // check if client exists
        Task<bool> ClientExistsAsync(int id);
    }
}
