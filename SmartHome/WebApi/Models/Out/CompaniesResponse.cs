using Domain;

namespace WebApi.Models.Out;

public class CompaniesResponse
{
    public List<CompanyResponse> Companies { get; set; }
    
    public CompaniesResponse(List<Company> companies)
    {
        Companies = companies.Select(company => new CompanyResponse(company)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is CompaniesResponse response &&
                Companies.SequenceEqual(response.Companies);
    }
}