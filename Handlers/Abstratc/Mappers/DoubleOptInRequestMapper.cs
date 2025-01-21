using Amazon.Common;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Models;
using Amazon.Models.Response;

namespace Amazon.Handlers.Abstratc.Mappers
{
	public static class DoubleOptInRequestMapper
	{
        public static OperationObjectResult<DoubleOptInModel> MapToDoubleInModel(OperationObjectResult<UserDALResponse> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok || response.Value == null)
                return OperationObjectResult<DoubleOptInModel>.CreateErrorResponse(response.Status, response.Message);

            var firstResponse = response.Value; // Prendiamo il primo elemento della lista, se esiste

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