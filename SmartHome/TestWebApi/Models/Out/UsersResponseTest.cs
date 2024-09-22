using Domain;
using WebApi.Out;

namespace TestWebApi.Models.Out;


public class UsersResponseTest
{
    private const string NameSample = "nameSample";
    private const string SurnameSample = "surnameSample";
    private const string EmailSample = "email@sample.com";
    private const string PasswordSample = "passwordSample1@";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    
    
    [TestMethod]
    public void AssignValues()
    {
        Administrator administrator = new Administrator(); 
        HomeOwner homeOwner = new HomeOwner();
        CompanyOwner companyOwner = new CompanyOwner(); 
        
        User user = new User()
        {
            Name = NameSample,
            Surname = SurnameSample,
            Email = EmailSample,
            Password = PasswordSample,
            Photo = ProfilePictureUrl,
            Roles = new List<Role>{administrator,homeOwner,companyOwner}
        };

        string fullName = NameSample + " " + SurnameSample; 

        UserResponse response = new UserResponse(user);

        Assert.IsTrue(
            user.Name == response.Name &&
            user.Surname == response.Surname &&
            fullName == response.FullName &&
            user.CreatedAt.Date == response.CreatedAt.Date &&
            user.Roles.Count == response.Roles.Count &&
            response.Roles.Contains(administrator) &&
            response.Roles.Contains(homeOwner) &&
            response.Roles.Contains(companyOwner)
        );

    }
    
    
    [TestMethod]
    public void AssignValuesWithoutRoles()
    {
        User user = new User()
        {
            Name = NameSample,
            Surname = SurnameSample,
            Email = EmailSample,
            Password = PasswordSample,
            Photo = ProfilePictureUrl,
            Roles = null
        };
        

        UserResponse response = new UserResponse(user);
        
        Assert.AreEqual(user.Roles.Count, response.Roles.Count);
    }
}