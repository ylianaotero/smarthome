using Domain;
using WebApi.In;

namespace TestWebApi.Models.In;

[TestClass]
public class CreateUserTest
{
    private const string NameSample = "nameSample";
    private const string SurnameSample = "surnameSample";
    private const string EmailSample = "email@sample.com";
    private const string PasswordSample = "passwordSample1@";
    private const string ProfilePictureUrl = "https://example.com/images/profile.jpg";
    private CreateHomeOwnerRequest _request;
    
    [TestInitialize]
    public void Init()
    {
        _request = new CreateHomeOwnerRequest()
        {
            Name = NameSample,
            Surname = SurnameSample,
            Email = EmailSample,
            Password = PasswordSample,
            Photo = ProfilePictureUrl
        };
    }
    
    [TestMethod]
    public void AssignsProperties()
    {
        Assert.IsTrue(
            _request is { Name: NameSample, Surname: SurnameSample } &&
            _request.Email == EmailSample &&
            _request.Password == PasswordSample &&
            _request.Photo == ProfilePictureUrl
        );

    }
    
    [TestMethod]
    public void UserRequestToUserEntity()
    {
        User entity = _request.ToEntity();

        Assert.IsTrue(
            entity.Name == NameSample &&
            entity.Surname == SurnameSample &&
            entity.Email == EmailSample &&
            entity.Password == PasswordSample &&
            entity.Photo == ProfilePictureUrl
        );

    }
}