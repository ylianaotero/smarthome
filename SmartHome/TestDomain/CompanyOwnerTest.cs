using Domain;

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
}