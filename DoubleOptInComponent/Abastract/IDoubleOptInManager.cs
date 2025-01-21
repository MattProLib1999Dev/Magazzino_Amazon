using Amazon.Common;
using Amazon.DoubleOptInComponent.Models;
using Amazon.DoubleOptInComponent.Models.Request.Response;

namespace Amazon.DoubleOptInComponent.Abastract
{
	public interface IDoubleOptInManager
	{
        public Task<OperationObjectResult<DoubleOptInModelResponse>> GenerateDoubleOptInToken(DoubleOptInModel input);

        public Task<OperationObjectResult<List<DoubleOptInModelResponse>>> VerifyDoubleOptInToken(DoubleOptInModel user);
	}
}