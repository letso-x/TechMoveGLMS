
using Microsoft.AspNetCore.Mvc;
using TechMoveGLMS.Models;
using TechMoveGLMS.Interfaces;
using System.Text.Json;

public class ContractsController : Controller
{
    private readonly IContractService _contractService;
    private readonly IWebHostEnvironment _environment;
    private readonly ICurrencyService _currencyService;

    // dependency injection for services
    public ContractsController(IContractService contractService, IWebHostEnvironment environment, ICurrencyService currencyService)
    {
        _contractService = contractService;
        _environment = environment;
        _currencyService = currencyService;
    }

    // GET: CONTRACTS 
    public async Task<IActionResult> Index(DateTime? startDate = null, DateTime? endDate = null, string status = null)    
    {
        // i think this is how you parse the status from the dropdown
        ContractStatus? parsedStatus = null;
        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse<ContractStatus>(status, out var s))
            {
                parsedStatus = s;
            }
        }

        // get contracts filtered by dates and status
        var contracts = await _contractService.GetAllContractsAsync(startDate, endDate, parsedStatus);

        // pass filter values to view so form stays populated
        ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
        ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
        ViewBag.Status = status;

        return View(contracts);
    }

    // GET: CONTRACTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get the contract from service
        var contract = await _contractService.GetContractByIdAsync(id.Value);
        if (contract == null)
        {
            return NotFound();
        }

        return View(contract);
    }

    // GET: CONTRACTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CONTRACTS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contract contract, IFormFile pdfFile)
    {
        if (ModelState.IsValid)
        {
            // check if user uploaded a file
            if (pdfFile != null)
            {
                var extension = Path.GetExtension(pdfFile.FileName);

                // only allow pdf files
                if (extension.ToLower() != ".pdf")
                {
                    ModelState.AddModelError("", "Only PDF files allowed.");
                    return View(contract);
                }

                // generate unique filename using guid
                var fileName = $"{Guid.NewGuid()}.pdf";

                // create path in uploads folder
                var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                // save file to server
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                // store filename in database
                contract.SignedAgreement = fileName;
            }

            // call service to create contract
            await _contractService.CreateContractAsync(contract);
            return RedirectToAction(nameof(Index));
        }

        return View(contract);
    }

    // GET: CONTRACTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get contract from service
        var contract = await _contractService.GetContractByIdAsync(id.Value);
        if (contract == null)
        {
            return NotFound();
        }
        return View(contract);
    }

    // POST: CONTRACTS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,StartDate,EndDate,Status,SignedAgreement,ServiceLevel,ClientId,Client,ServiceRequests")] Contract contract, IFormFile pdfFile)
    {
        if (id != contract.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // handle new file upload if provided
                if (pdfFile != null)
                {
                    var extension = Path.GetExtension(pdfFile.FileName);
                    if (extension.ToLower() != ".pdf")
                    {
                        ModelState.AddModelError("", "Only PDF files allowed.");
                        return View(contract);
                    }

                    var fileName = $"{Guid.NewGuid()}.pdf";
                    var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(stream);
                    }

                    contract.SignedAgreement = fileName;
                }

                // update contract using service
                await _contractService.UpdateContractAsync(contract);
            }
            catch
            {
                // if contract doesnt exist anymore
                if (!await _contractService.ContractExistsAsync(contract.Id))
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
        return View(contract);
    }

    // GET: CONTRACTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get contract from service
        var contract = await _contractService.GetContractByIdAsync(id.Value);
        if (contract == null)
        {
            return NotFound();
        }

        return View(contract);
    }

    // POST: CONTRACTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        // use service to delete
        await _contractService.DeleteContractAsync(id.Value);
        return RedirectToAction(nameof(Index));
    }

    // download the pdf file for a contract
    public async Task<IActionResult> Download(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // get contract from service
        var contract = await _contractService.GetContractByIdAsync(id.Value);
        if (contract == null || string.IsNullOrEmpty(contract.SignedAgreement))
        {
            return NotFound();
        }

        // build file path
        var filePath = Path.Combine(_environment.WebRootPath, "uploads", contract.SignedAgreement);

        // check if file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        // read file and return it
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/pdf", contract.SignedAgreement);
    }

    // convert currency - used by javascript
    [HttpPost]
    public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        // call currency service to get the converted amount
        var converted = await _currencyService.ConvertCurrency(request.Amount);

        return Json(new { result = converted });
    }
}
