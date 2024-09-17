using Domain;
using Domain.Exceptions.CompanyOwner;

namespace DomainTest;

[TestClass]
public class CompanyOwnerTest
{
    [TestMethod]
    public void TestAddCompanyToCompanyOwner()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner(company);
        Assert.AreEqual(company,companyOwner.Company);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ACompanyHasAlreadyBeenRegistredException))]
    public void TestCannotAddCompanyToCompanyOwner()
    {
        Company company = new Company();
        CompanyOwner companyOwner = new CompanyOwner(company);
        Company company2 = new Company();
        companyOwner.Company = company2;
    }
    
    [TestMethod]
    public void TestAddIncompleteCompany()
    {
        Company company = new Company();
        Assert.AreEqual(company,company.HasACompleteCompany);
    }
}