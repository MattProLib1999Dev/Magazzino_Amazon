using Amazon.AccessTokenComponent.Model;
using Amazon.Common;

namespace Amazon.Handlers.Abstratc
{
	public interface IAccessTokenManager
	{
        public Task<OperationObjectResult<AccessTokenEncriptModel>> GenerateToken(AccessTokenModel input);
        public Task<OperationObjectResult<AccessTokenModel>> Validate(AccessTokenEncriptModel accessTokenEncriptModel);
	 
	}
}