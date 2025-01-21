using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;

namespace Amazon.AccessTokenComponent.Model.Abstract
{
	public interface IAccessTokenManager
	{
        public Task<OperationObjectResult<AccessTokenEncriptModel>> GenerateToken(List<AccessTokenModel> user);

        public Task<OperationObjectResult<AccessTokenModelRequest>> Validate(List<AccessTokenModelRequest> accessTokenEncriptModel);
	}
}