using TechMoveGLMS.Models;

namespace TechMoveGLMS.Interfaces
{
    public interface IServiceRequestService
    {
        Task CreateRequest(ServiceRequest request);
    }
}
