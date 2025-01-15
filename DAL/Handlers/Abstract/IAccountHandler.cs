using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Amazon.Models.Response;

namespace Amazon.Appunti.Handlers.Abstract
{
  public interface IAccountHandler
  {
    public Task<List<UserDALResponse>> GetAllUsers();
    public Task<OperationObjectResult<List<UserDALResponse>>> UserInfo(UserInfoHandlerRequest request);
    public Task<OperationObjectResult<LoginHandlerResponse>> Login(LoginHandlerRequest request);

  }
}