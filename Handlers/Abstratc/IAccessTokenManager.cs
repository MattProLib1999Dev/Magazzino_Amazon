using Amazon.AccessTokenComponent.Model;
using Amazon.Common;
using Amazon.DAL.Models.Response;

namespace Amazon.Handlers.Abstratc
{
	public interface IAccessTokenManager
	{
        Task<OperationObjectResult<List<AccessTokenEncriptModel>>> GenerateToken(List<AccessTokenModel> user);
        Task<OperationObjectResult<AccessTokenModel>> Validate(AccessTokenEncriptModel accessTokenEncriptModel);
	 
	}
}