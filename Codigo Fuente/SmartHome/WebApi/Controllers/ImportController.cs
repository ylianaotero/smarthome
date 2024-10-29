using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/v1/imports")]
[ApiController]
public class ImportController(IImporter.IImporter importer) : ControllerBase
{
    private IImporter.IImporter _importer = importer;
    
    [HttpGet]
    public IActionResult GetNames([FromHeader] string dllPath)
    {
        try
        {
            return Ok(this._importer.GetImplementationsNames(dllPath));
        }
        catch (Exception ex)
        {
            return NotFound("Either such directory could not be found, or the file name is not correct.");
        }
    }
    
    
}