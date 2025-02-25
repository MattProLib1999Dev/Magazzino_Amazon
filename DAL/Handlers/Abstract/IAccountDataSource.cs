using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;

namespace Amazon.Appunti.Handlers.Abstract
{
	public interface IAccountDataSource
	{
	  public Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers();
	  public Task<OperationObjectResult<UserDALResponse>> UserInfo(UserInfoHandlerRequest request);
	  public Task<OperationObjectResult<List<UserDALResponse>>> Login(List<LoginHandlerRequest> request);
	  public Task<OperationObjectResult<UserDALResponse>> CreateUser(List<CreateUserHandlerRequest> request);
	  public Task<OperationObjectResult<ConfirmUserHandlerResponse>> ConfirmUser(List<ConfirmUserHandlerRequest> request);

    }

}