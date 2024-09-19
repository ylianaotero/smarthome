using Domain;

namespace TestWebApi.Out;

public class AdminResponseTest
{
    private readonly string nameSample = "nameSample";
    private readonly string surnameSample = "surnameSample";
    private readonly string emailSample = "email@sample.com";
    private readonly string passwordSample = "passwordSample1@";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    [TestMethod]
    public void AssignValues()
    {
        Administrator administrator = new Administrator(); 
        HomeOwner homeOwner = new HomeOwner();
        CompanyOwner companyOwner = new CompanyOwner(); 
        User user = new User()
        {
            Name = nameSample,
            Surname = surnameSample,
            Email = emailSample,
            Password = passwordSample,
            Photo = ProfilePictureUrl,
            Roles = new List<Role>{administrator,homeOwner,companyOwner}
        };

        string fullName = nameSample + " " + surnameSample; 

        AdminResponse response = new AdminResponse(user);

        Assert.AreEqual(user.Name, response.Name);
        Assert.AreEqual(user.Surname, response.Surname);
        Assert.AreEqual(user.Email, response.Email);
        

    }
    
    
}