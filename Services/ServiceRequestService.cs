using TechMoveGLMS.Data;
using TechMoveGLMS.Interfaces;
using TechMoveGLMS.Models;
using Microsoft.EntityFrameworkCore;



namespace TechMoveGLMS.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly AppDbContext _context;

        public ServiceRequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateRequest(ServiceRequest request)
        {
            if (request == null)
            {
                throw new Exception("Request cannot be null");
            }

            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == request.ContractId);

            if (contract == null)
            {
                throw new Exception("Contract not found.");
            }

            var state = ContractStateFactory.Create(contract.Status);

            if(!state.CanCreateRequest())
            {
                throw new Exception("Cannot create request for inactive contract");
            }

            _context.ServiceRequests.Add(request);

            await _context.SaveChangesAsync();


           }
    }
}
