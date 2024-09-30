using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;


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

    public List<Company>? GetAllCompanies()
    {
        throw new NotImplementedException();
    }
}