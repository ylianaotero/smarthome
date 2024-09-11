namespace DomainTest;

[TestClass]
public class HomeOwnerTest
{
    [TestMethod]
    public void TestAddNameToHomeOwner()
    {
        // Arrange
        HomeOwner homeOwner = new HomeOwner();
        
        // Act
        homeOwner.Name = "Pedro";

        // Assert
        Assert.AreEqual("Pedro", homeOwner.Name);
    }
}