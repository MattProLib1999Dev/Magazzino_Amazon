using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Amazon.Models.Response;

namespace Amazon;

public class UserRepository : IUserService
{

    private ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public ConfirmUserHandlerResponse ConfirmUser(ConfirmUserHandlerRequest request)
    {
        return new ConfirmUserHandlerResponse();
    }

    public CreateUserHandlerResponse CreateUser(List<UserDALResponse> request)
    {
        return new CreateUserHandlerResponse();
    }

    public List<UserDALResponse> GetAllUsers()
    {
        return new List<UserDALResponse>();
    }

    public Task<UserDALResponse> GetUser(int idUser)
    {
        return Task.FromResult(new UserDALResponse());
    }


    public string Login(LoginHandlerRequest request)
    {
        return "ti sei loggato con successo!";
    }

    public List<UserDALResponse> UserInfo(UserInfoHandlerRequest request)
    {
        return new List<UserDALResponse>();
    }

    UserDALResponse IUserService.GetUser(int idUser)
    {
        throw new NotImplementedException();
    }
}

