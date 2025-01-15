namespace Amazon.DAL.Handlers.Models.Response.Response
{
	public class AccountResponseMapper
	{
         public static OperationObjectResult<LoginModelResponse> MapFromLoginHandlerResponse(OperationObjectResult<LoginHandlerResponse> response)
         {
            if (response.Status != OperationObjectResult.Ok)
             return OperationObjectResult<LoginModelResponse>.CreateErrorResponse(response.Status, response.Message);
            return OperationObjectResult<LoginModelResponse>.CreateCorrecrResponse(new LoginModelResponse)
            {
                AccessToken = response.Value.AccessToken
            }
         }
	}
}