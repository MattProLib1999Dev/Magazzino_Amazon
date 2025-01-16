using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;

namespace Amazon.AccessTokenComponent.Model.Abstract
{
	public interface IAccessTokenManager
	{
        public Task<OperationObjectResult<AccessTokenEncriptModel>> GenerateToken(AccessTokenModel user);

        public Task<OperationObjectResult<AccessTokenModelRequest>> Validate(AccessTokenModelRequest accessTokenEncriptModel);
	}
}