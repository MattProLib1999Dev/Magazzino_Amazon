using Amazon.AccessTokenComponent.Model;
using Amazon.AccessTokenComponent.Model.Dummy;
using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;
using Amazon.Handlers.Abstratc;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.AccessTokenComponent
{
    public class DummyAccessToken : IAccessTokenManager
    {

        private const string AccesstokenConstant = "AccessToken_";

        private const string RefreshTokenCostant = "RefreshToken_";

        private const int TimeInCacheRefreshToken = 60;

        private readonly IMemoryCache memoryCache;
        private const int TimeInCache = 1;

        public DummyAccessToken(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task<OperationObjectResult<List<AccessTokenEncriptModel>>> GenerateToken(AccessTokenModel input)
        {
            if (input == null || input.IdUser < 1 || string.IsNullOrEmpty(input.UserName))
            {
                return Task.FromResult(
                    OperationObjectResult<List<AccessTokenEncriptModel>>.CreateErrorResponse(
                        OperationObjectResultStatus.BadRequest
                    )
                );
            }

            var accessToken = Guid.NewGuid().ToString("N");

            memoryCache.Set(accessToken, input, TimeSpan.FromMinutes(TimeInCache));

            var accessTokenEncryptModels = new List<AccessTokenEncriptModel>
            {
                new AccessTokenEncriptModel
                {
                    Accesstoken = accessToken,
                    IdUser = input.IdUser
                }
            };

            return Task.FromResult(
                OperationObjectResult<List<AccessTokenEncriptModel>>.CreateCorrectResponseGeneric(accessTokenEncryptModels)
            );
        }


        public Task<OperationObjectResult<AccessTokenModel>> Validate(AccessTokenModelRequest accessTokenEncriptModel)
        {
            var exist = memoryCache.TryGetValue(accessTokenEncriptModel.AccessToken, out var result);

            if (exist)
                return Task.FromResult(OperationObjectResult<AccessTokenModel>.CreateCorrectResponseGeneric((AccessTokenModel)result));
            return Task.FromResult(OperationObjectResult<AccessTokenModel>.CreateErrorResponse(OperationObjectResultStatus.NotFound));
        }

        public Task<OperationObjectResult<AccessTokenEncriptModelResponse>> GenerateTokenFromRefreshToken(RefreshTokenModelRequest refreshTokenRequest)
        {
            var accessToken_key = $"{AccesstokenConstant}{refreshTokenRequest.AccessToken}";
            var refreshToken_key = $"{RefreshTokenCostant}{refreshTokenRequest.AccessToken}";
            var exist = memoryCache.TryGetValue(refreshToken_key, out RefreshTokenObject resultRefreshTokenObject);

            if (exist && refreshTokenRequest.AccessToken.Equals(resultRefreshTokenObject.AccessToken, StringComparison.InvariantCulture))
            {
                memoryCache.Remove(accessToken_key);
                memoryCache.Remove(refreshToken_key);

                var accessToken = Guid.NewGuid().ToString("N");
                var refreshToken = Guid.NewGuid().ToString("N");

                accessToken_key = $"{AccesstokenConstant}{accessToken}";
                refreshToken_key = $"{AccesstokenConstant}{accessToken}";

                var refreshTokenObject = RefreshTokenObject.Create(accessToken, resultRefreshTokenObject.AccessTokenModel);

                memoryCache.Set(refreshToken_key, refreshTokenObject, TimeSpan.FromMinutes(TimeInCacheRefreshToken));
                memoryCache.Set(refreshToken_key, resultRefreshTokenObject.AccessTokenModel, TimeSpan.FromMinutes(TimeInCache));

                return Task.FromResult(OperationObjectResult<AccessTokenEncriptModelResponse>.CreateCorrectResponseGeneric(new AccessTokenEncriptModelResponse { Accesstoken = accessToken }));

            }

            return Task.FromResult(OperationObjectResult<AccessTokenEncriptModelResponse>.CreateErrorResponse(OperationObjectResultStatus.NotFound));


        }

        public Task<OperationObjectResult<List<AccessTokenEncriptModel>>> GenerateToken(List<AccessTokenModel> input)
        {
            throw new NotImplementedException();
        }
    }
}
