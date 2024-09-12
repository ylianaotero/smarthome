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
    
    [TestMethod]
    public void TestAddSurnameToCompanyOwner()
    {
        // Arrange
        CompanyOwner companyOwner = new CompanyOwner();
        
        // Act
        companyOwner.Surname = "Gutierrez";

        // Assert
        Assert.AreEqual("Gutierrez", companyOwner.Surname);
    }
    
    [TestMethod]
    public void TestAddEmailToCompanyOwner()
    {
        // Arrange
        CompanyOwner companyOwner = new CompanyOwner();
        
        // Act
        companyOwner.Email = "frangutierrez@gmail.com";

        // Assert
        Assert.AreEqual("frangutierrez@gmail.com", companyOwner.Email);
    }
    
    [TestMethod]
    public void TestAddPasswordToCompanyOwner()
    {
        // Arrange
        CompanyOwner companyOwner = new CompanyOwner();
        
        // Act
        companyOwner.Password = "password1";

        // Assert
        Assert.AreEqual("password1", companyOwner.Password);
    }
}