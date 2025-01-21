using Amazon.AccessTokenComponent.Model;
using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;

namespace Amazon.Handlers.Abstratc
{
	public interface IAccessTokenManager
	{
        public Task<OperationObjectResult<List<AccessTokenEncriptModel>>> GenerateToken(List<AccessTokenModel> input);
        public Task<OperationObjectResult<AccessTokenModel>> Validate(AccessTokenModelRequest accessTokenModelRequest);
        public Task<OperationObjectResult<AccessTokenEncriptModelResponse>> GenerateTokenFromRefreshToken(RefreshTokenModelRequest accessTokenModelRequest);
    }
}