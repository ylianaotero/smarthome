using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.In;
using WebApi.Models.Out;


namespace WebApi.Controllers;


[Route("api/v1/companies")]
[ApiController]
public class CompanyController : ControllerBase
{
  
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public IActionResult GetCompanies([FromQuery] CompanyRequest request)
    {
        CompaniesResponse companiesResponse = new CompaniesResponse(_companyService.GetCompaniesByFilter(request.ToFilter()));
        
        return Ok(companiesResponse);
    }
}