using Amazon.Common;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.DoubleOptInComponent.Models;
using Amazon.DoubleOptInComponent.Models.Request.Response;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon.DoubleOptInComponent.Dummy
{
    public class DummyDoubleOptIn : IDoubleOptInManager
    {

        private const string DoubleOptInConstant = "DoubleOptIn_";
        private readonly IMemoryCache memoryCache;
        private const int TimeCache = 1;

        public DummyDoubleOptIn(IMemoryCache inputMemoryCache)
        {
            memoryCache = inputMemoryCache;
        }

        public Task<OperationObjectResult<DoubleOptInModelResponse>> GenerateDoubleOptInToken(DoubleOptInModel input)
        {
            if (input == null || input.IdUser < 1 || string.IsNullOrEmpty(input.Username))
                return Task.FromResult(OperationObjectResult<DoubleOptInModelResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest));
            var doubleOptInToken_key = $"{DoubleOptInConstant}{input.DoubleOptInToken}";
            memoryCache.Set(doubleOptInToken_key, input, TimeSpan.FromMinutes(TimeCache));
            return Task.FromResult(OperationObjectResult<DoubleOptInModelResponse>.CreateCorrectResponseGeneric(
                new DoubleOptInModelResponse
                {
                    DoubleOptInToken = input.DoubleOptInToken,
                    Username = input.Username
                }
            ));
        }

        public Task<OperationObjectResult<List<DoubleOptInModelResponse>>> VerifyDoubleOptInToken(DoubleOptInModel user)
        {
            throw new NotImplementedException();
        }
    }
}