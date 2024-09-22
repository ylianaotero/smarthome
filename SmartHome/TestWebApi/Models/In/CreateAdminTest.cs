using Domain;
using WebApi.In;

namespace TestWebApi.Models.In;

public class CreateAdminTest
{
    private const string NameSample = "nameSample";
    private const string SurnameSample = "surnameSample";
    private const string EmailSample = "email@sample.com";
    private const string PasswordSample = "passwordSample1@";
    private CreateAdminRequest _request;
    
    [TestInitialize]
    public void Init()
    {
        _request = new CreateAdminRequest()
        {
            Name = NameSample,
            Surname = SurnameSample,
            Email = EmailSample,
            Password = PasswordSample
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.IsTrue(
            _request.Name == NameSample &&
            _request.Surname == SurnameSample &&
            _request.Email == EmailSample &&
            _request.Password == PasswordSample
        );
    }
    
    [TestMethod]
    public void AdminRequestToUserEntity()
    {
        User entity = _request.ToEntity();

        Assert.IsTrue(
            entity.Name == NameSample &&
            entity.Surname == SurnameSample &&
            entity.Email == EmailSample &&
            entity.Password == PasswordSample &&
            entity.Roles.Any(role => role is Administrator)
        );
    }
}