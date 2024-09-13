using Domain;

namespace DomainTest;

[TestClass]
public class CompanyOwnerTest
{
    [TestMethod]
    public void TestAddCompanyToCompanyOwner()
    {
        CompanyOwner companyOwner = new CompanyOwner();
        Company company = new Company();
        Assert.AreEqual(company,companyOwner.Company);
    }
}