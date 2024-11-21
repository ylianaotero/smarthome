using CustomExceptions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.Out;
using WebApi.Attributes;

namespace WebApi.Controllers;

[Route("api/v1/imports")]
[ApiController]
public class ImportController : ControllerBase
{
    private IImporter.IImporter _importer;
    private ISessionService _sessionService;
    private ICompanyService _companyService;
    
    public ImportController(IImporter.IImporter importer, ISessionService sessionService, ICompanyService companyService)
    {
        this._importer = importer;
        this._sessionService = sessionService;
        this._companyService = companyService;
    }

    private const string RoleWithPermissions = "CompanyOwner";
    private const string AuthorizationHeader = "Authorization";
    private const string NotFoundMessage = "Not found";
    private const string OkMessage = "Ok";

    [HttpGet]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult GetNames()
    {
        try
        {
            List<ImportResponse> implementations = this._importer.GetImplementationsNamesAndPath();

            return Ok(implementations);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost]
    [Route("new")]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult PostImporter([FromBody] PostImportRequest path)
    {
        if (this._importer.MoveDllFile(path.Path))
        {
            return Ok();
        }else{
            
            return NotFound();
        }
    }
    
    
    

    [HttpPost]
    [RolesWithPermissions(RoleWithPermissions)]
    public IActionResult Import([FromBody] ImportRequest importRequest)
    {
        try
        {
            string authHeader = HttpContext.Request.Headers[AuthorizationHeader];

            if (Guid.TryParse(authHeader, out Guid tokenGuid))
            {
                if (this._importer.Import(importRequest.DllPath, importRequest.FilePath, importRequest.Type,
                        _companyService.GetCompaniesOfOwners(_sessionService.GetUserId(tokenGuid))))
                {
                    return Ok(OkMessage);
                }
            }

            return NotFound(NotFoundMessage);
        }
        catch (ElementNotFound elementNotFound)
        {
            return NotFound(elementNotFound.Message);
        }
        catch (CannotFindItemInList cannotFindItemInList)
        {
            return NotFound(cannotFindItemInList.Message);
        }
    }
}
