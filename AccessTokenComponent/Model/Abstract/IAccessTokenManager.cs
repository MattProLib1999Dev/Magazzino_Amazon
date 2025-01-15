using Amazon.Common;

namespace Amazon.AccessTokenComponent.Model.Abstract
{
	public interface IAccessTokenManager
	{
        Task<OperationObjectResult<AccessTokenEncriptModel>> GenerateToken(AccessTokenModel user);

        Task<OperationObjectResult<AccessTokenModel>> Validate(AccessTokenEncriptModel accessTokenEncriptModel);
	}
}