
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Models;
using TechMoveGLMS.Data;
using TechMoveGLMS.Interfaces;

public class ServiceRequestsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IServiceRequestService _service;

    // inject dependencies
    public ServiceRequestsController(AppDbContext context, IServiceRequestService service)
    {
        _context = context;
        _service = service;
    }

    // GET: SERVICEREQUESTS 
    public async Task<IActionResult> Index()    
    {
        var requests = await _context.ServiceRequests
            .Include(sr => sr.Contract)
            .ToListAsync();
        return View(requests);
    }

    // GET: SERVICEREQUESTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequest = await _context.ServiceRequests
            .Include(sr => sr.Contract)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (servicerequest == null)
        {
            return NotFound();
        }

        return View(servicerequest);
    }

    // GET: SERVICEREQUESTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SERVICEREQUESTS/Create 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Description,ContractId,Cost,Status,Contract")] ServiceRequest servicerequest)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // service handles validation (contract status check)
                await _service.CreateRequest(servicerequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
        return View(servicerequest);
    }

    // GET: SERVICEREQUESTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequest = await _context.ServiceRequests
            .Include(sr => sr.Contract)
            .FirstOrDefaultAsync(sr => sr.Id == id);
        if (servicerequest == null)
        {
            return NotFound();
        }
        return View(servicerequest);
    }

    // POST: SERVICEREQUESTS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Description,ContractId,Cost,Status,Contract")] ServiceRequest servicerequest)
    {
        if (id != servicerequest.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servicerequest);
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!ServiceRequestExists(servicerequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(servicerequest);
    }

    // GET: SERVICEREQUESTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequest = await _context.ServiceRequests
            .Include(sr => sr.Contract)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (servicerequest == null)
        {
            return NotFound();
        }

        return View(servicerequest);
    }

    // POST: SERVICEREQUESTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var servicerequest = await _context.ServiceRequests.FindAsync(id);
        if (servicerequest != null)
        {
            _context.ServiceRequests.Remove(servicerequest);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ServiceRequestExists(int id)
    {
        return _context.ServiceRequests.Any(e => e.Id == id);
    }
}
