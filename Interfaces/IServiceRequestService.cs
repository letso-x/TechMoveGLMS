using TechMoveGLMS.Models;

namespace TechMoveGLMS.Interfaces
{
    public interface IServiceRequestService
    {
        Task<List<ServiceRequest>> GetAll();
        Task<ServiceRequest> GetById(int id);
        Task CreateRequest(ServiceRequest request);
        Task Update(int id, ServiceRequest request);
        Task Delete(int id);
    }
}
