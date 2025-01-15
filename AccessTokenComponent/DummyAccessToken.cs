using Amazon.AccessTokenComponent.Model;
using Amazon.Common;
using Amazon.Handlers.Abstratc;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.AccessTokenComponent
{
    public class DummyAccessToken : IAccessTokenManager
    {
        private readonly IMemoryCache memoryCache;
        private const int TimeInCache = 3;

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


        public Task<OperationObjectResult<AccessTokenModel>> Validate(AccessTokenEncriptModel accessTokenEncriptModel)
        {
            var exist = memoryCache.TryGetValue(accessTokenEncriptModel.Accesstoken, out var result);

            if (exist && result is AccessTokenModel accessTokenModel)
            {
                return Task.FromResult(OperationObjectResult<AccessTokenModel>.CreateCorrectResponseGeneric(accessTokenModel));
            }
            return Task.FromResult(OperationObjectResult<AccessTokenModel>.CreateErrorResponse(OperationObjectResultStatus.NotFound));
        }

        
    }
}
