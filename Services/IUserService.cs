using Amazon;
using Amazon.Common;
using Amazon.Controllers;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Amazon.Models.Response;
using Amazon.Prodotto;

public interface IUserService
{
    public List<UserDALResponse> GetAllUsers();
    public UserDALResponse GetUser(int idUser);
    public List<UserDALResponse> UserInfo(UserInfoHandlerRequest request);
    public string Login(LoginHandlerRequest request);
    public ConfirmUserHandlerResponse ConfirmUser(ConfirmUserHandlerRequest request);
    public CreateUserHandlerResponse CreateUser(List<UserDALResponse> request);
}