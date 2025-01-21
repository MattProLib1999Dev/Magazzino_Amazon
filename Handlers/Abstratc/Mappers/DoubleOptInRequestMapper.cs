using Amazon.Common;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DoubleOptInComponent.Models;
using Amazon.Models.Response;

namespace Amazon.Handlers.Abstratc.Mappers
{
	public static class DoubleOptInRequestMapper
	{
        public static OperationObjectResult<DoubleOptInModel> MapToDoubleInModel(OperationObjectResult<List<CreateUserHandlerResponse>> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok || response.Value == null || !response.Value.Any())
                return OperationObjectResult<DoubleOptInModel>.CreateErrorResponse(response.Status, response.Message);

            var firstResponse = response.Value.First(); // Prendiamo il primo elemento della lista, se esiste

            return OperationObjectResult<DoubleOptInModel>.CreateCorrectResponseGeneric(
                new DoubleOptInModel
                {
                    IdUser = firstResponse.IdUser,
                    Username = firstResponse.Username,
                    DoubleOptInToken = firstResponse.DoubleOptInToken
                });
        }


	}
}