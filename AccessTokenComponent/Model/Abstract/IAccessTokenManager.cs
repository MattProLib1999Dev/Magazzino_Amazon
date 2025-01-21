using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;

namespace Amazon.AccessTokenComponent.Model.Abstract
{
	public interface IAccessTokenManager
	{
        public Task<OperationObjectResult<AccessTokenEncriptModel>> GenerateToken(List<AccessTokenModel> user);
        public Task<OperationObjectResult<AccessTokenModelRequest>> Validate(List<AccessTokenModelRequest> accessTokenEncriptModel);
		public Task<OperationObjectResult<ConfirmUserHandlerResponse>> ConfirmUser(List<ConfirmCreateUserDALRequest> request);

	}
}