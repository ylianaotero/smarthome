using Domain;

namespace DomainTest;

[TestClass]
public class CompanyOwnerTest
{
    [TestMethod]
    public void TestAddNameToCompanyOwner()
    {
        // Arrange
        CompanyOwner companyOwner = new CompanyOwner();
        
        // Act
        companyOwner.Name = "Francisco";

        // Assert
        Assert.AreEqual("Francisco", companyOwner.Name);
    }
}