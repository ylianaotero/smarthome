using Domain;
using WebApi.Out;

namespace TestWebApi.Out;


public class UsersResponseTest
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

        UserResponse response = new UserResponse(user);

        Assert.AreEqual(user.Name, response.Name);
        Assert.AreEqual(user.Surname, response.Surname);
        Assert.AreEqual(fullName, response.FullName);
        Assert.AreEqual(user.CreatedAt.Date, response.CreatedAt.Date);
        Assert.AreEqual(user.Roles.Count, response.Roles.Count);
        
        
        Assert.IsTrue(response.Roles.Contains(administrator));
        Assert.IsTrue(response.Roles.Contains(homeOwner));
        Assert.IsTrue(response.Roles.Contains(companyOwner));
    }
    
    
    [TestMethod]
    public void AssignValuesWithoutRoles()
    {
        User user = new User()
        {
            Name = nameSample,
            Surname = surnameSample,
            Email = emailSample,
            Password = passwordSample,
            Photo = ProfilePictureUrl,
            Roles = null
        };
        

        UserResponse response = new UserResponse(user);
        
        Assert.AreEqual(user.Roles.Count, response.Roles.Count);
    }
}