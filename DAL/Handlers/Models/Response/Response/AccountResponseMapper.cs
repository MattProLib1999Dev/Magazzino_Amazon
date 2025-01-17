using Amazon.Common;
using Amazon.Models.Response;

namespace Amazon.DAL.Handlers.Models.Response.Response
{
	public class AccountResponseMapper
	{
        public static OperationObjectResult<UserInfoModelResponse> MapFromLoginHandlerResponse(OperationObjectResult<LoginHandlerResponse> response)
{
    // Controllo dello stato della risposta
    if (response.Status != OperationObjectResultStatus.Ok)
    {
        return OperationObjectResult<UserInfoModelResponse>.CreateErrorResponse(response.Status, response.Message);
    }

    // Controllo del valore della risposta
    if (response.Value == null)
    {
        return OperationObjectResult<UserInfoModelResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest, "Il valore della risposta è null.");
    }

    // Creazione della risposta mappata
    var responseValue = response.Value;
    var userInfoModelResponse = new UserInfoModelResponse
    {
        IdUser = responseValue.IdUser,
        Name = responseValue.Name,
        Surname = responseValue.Surname,
        Username = responseValue.Username
    };

    return OperationObjectResult<UserInfoModelResponse>.CreateCorrectResponseGeneric(userInfoModelResponse);
}


        
    }
}