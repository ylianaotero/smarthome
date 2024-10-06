using Domain.Concrete;

namespace Model.Out;

public class GetCompaniesResponse
{
    public List<GetCompanyResponse> Companies { get; set; }
    
    public GetCompaniesResponse(List<Company> companies)
    {
        Companies = companies.Select(company => new GetCompanyResponse(company)).ToList();
    }
    
    public override bool Equals(object? obj)
    {
        return obj is GetCompaniesResponse response &&
                Companies.SequenceEqual(response.Companies);
    }
}