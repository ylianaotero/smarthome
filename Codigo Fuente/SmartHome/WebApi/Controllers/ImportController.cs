using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/imports")]
[ApiController]
public class ImportController(IImporter.IImporter importer, ISessionService sessionService) : ControllerBase
{
    private IImporter.IImporter _importer = importer;
    private ISessionService _sessionService = sessionService;
    
    private const string RoleWithPermissions = "CompanyOwner";
    
    [HttpGet]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult GetNames([FromQuery] string dllPath)
    {
        try
        {
            Console.WriteLine(dllPath);
            List<ImportResponse> implementations = this._importer.GetImplementationsNamesAndPath(dllPath);
            
            foreach (var imp in implementations)
            {
                Console.WriteLine($"LOL  Implementación: {imp.ImplementationName}, Ubicación: {imp.AssemblyLocation}");
            }

            return Ok(implementations);
        }
        catch (Exception ex)
        {
            return NotFound("Either such directory could not be found, or the file name is not correct.");
        }
    }

    
    [HttpPost]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult Import([FromBody] ImportRequest importRequest)
    {
        try
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];

            if (Guid.TryParse(authHeader, out Guid tokenGuid))
            {
                if (this._importer.Import(importRequest.DllPath, importRequest.FilePath, importRequest.Type,_sessionService.GetCompanyOwnerRole(tokenGuid) ))
                {
                    return Ok("Ok");
                }
            }
            

            return NotFound("Either such directory could not be found, or the file name is not correct.");
        }
        catch (CannotAccessItem cannotAccessItem)
        {
            return StatusCode(StatusCodes.Status403Forbidden, cannotAccessItem.Message);
        }
        catch (ElementNotFound elementNotFound)
        {
            return NotFound(elementNotFound.Message);
        }
        
    }
    
    
    
    
    
    
}