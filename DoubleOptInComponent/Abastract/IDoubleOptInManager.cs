using Amazon.Common;
using Amazon.DoubleOptInComponent.Models;
using Amazon.DoubleOptInComponent.Models.Request.Response;

namespace Amazon.DoubleOptInComponent.Abastract
{
	public interface IDoubleOptInManager
	{
        public Task<OperationObjectResult<List<DoubleOptInModelResponse>>> GenerateDoubleOptInToken(OperationObjectResult<DoubleOptInModel> user);
        public Task<OperationObjectResult<List<DoubleOptInModelResponse>>> VerifyDoubleOptInToken(DoubleOptInModel user);
	}
}