
using Microsoft.AspNetCore.Mvc;
using TechMoveGLMS.Models;
using TechMoveGLMS.Interfaces;

public class ServiceRequestsController : Controller
{
    private readonly IServiceRequestService _service;

    // inject dependencies
    public ServiceRequestsController(IServiceRequestService service)
    {
        _service = service;
    }

    // GET: SERVICEREQUESTS 
    public async Task<IActionResult> Index()    
    {
        var requests = await _service.GetAll();
        return View(requests);
    }

    // GET: SERVICEREQUESTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequest = await _service.GetById(id.Value);
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

        var servicerequest = await _service.GetById(id.Value);
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
                await _service.Update(servicerequest.Id, servicerequest);
            }
            catch
            {
                throw;
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

        var servicerequest = await _service.GetById(id.Value);
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
        if (id == null)
        {
            return NotFound();
        }

        await _service.Delete(id.Value);
        return RedirectToAction(nameof(Index));
    }
}
