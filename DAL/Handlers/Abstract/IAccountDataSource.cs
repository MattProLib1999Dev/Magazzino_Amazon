using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Models.Response;

namespace Amazon.Appunti.Handlers.Abstract
{
	public interface IAccountDataSource
	{
	  public Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers();
	  Task<OperationObjectResult<List<UserDALResponse>>> UserInfo(UserInfoHandlerRequest request);
	  public Task<OperationObjectResult<OperationObjectResult<UserDALResponse>>> Login(LoginDALRequest request);

    }
}