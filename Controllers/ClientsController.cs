
using Microsoft.AspNetCore.Mvc;
using TechMoveGLMS.Models;
using TechMoveGLMS.Interfaces;

public class ClientsController : Controller
{
    private readonly IClientService _clientService;

    // inject the client service
    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    // GET: CLIENTS - get all clients
    public async Task<IActionResult> Index()    
    {
        var clients = await _clientService.GetAllClientsAsync();
        return View(clients);
    }

    // GET: CLIENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get the client from service
        var client = await _clientService.GetClientByIdAsync(id.Value);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // GET: CLIENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CLIENTS/Create - create new client
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Region,Contracts")] Client client)
    {
        if (ModelState.IsValid)
        {
            // use service to create client
            await _clientService.CreateClientAsync(client);
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }

    // GET: CLIENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get client from service
        var client = await _clientService.GetClientByIdAsync(id.Value);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    // POST: CLIENTS/Edit/5 - update client
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Email,PhoneNumber,Region,Contracts")] Client client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // update using service
                await _clientService.UpdateClientAsync(client);
            }
            catch
            {
                // check if client still exists
                if (!await _clientService.ClientExistsAsync(client.Id))
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
        return View(client);
    }

    // GET: CLIENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get client from service
        var client = await _clientService.GetClientByIdAsync(id.Value);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // POST: CLIENTS/Delete/5 - delete a client
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        // delete using service
        await _clientService.DeleteClientAsync(id.Value);
        return RedirectToAction(nameof(Index));
    }
}
