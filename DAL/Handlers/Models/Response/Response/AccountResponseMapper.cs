using Amazon.Common;
using Amazon.Models.Response;

namespace Amazon.DAL.Handlers.Models.Response.Response
{
	public class AccountResponseMapper
	{
         public static OperationObjectResult<UserInfoModelResponse> MapFromLoginHandlerResponse(OperationObjectResult<List<Amazon.Models.Response.LoginHandlerResponse>> response)
         {
            if (response.Status != OperationObjectResultStatus.Ok)
             return OperationObjectResult<UserInfoModelResponse>.CreateErrorResponse(response.Status, response.Message);
             var userInfoModelResponse = new UserInfoModelResponse();
             var responseValue = response.Value;
             var userInfoModelResponseObj = userInfoModelResponse;

            for (int i = 0; i < 0; i++)
            {
                userInfoModelResponseObj.IdUser = userInfoModelResponseObj.IdUser;
                userInfoModelResponseObj.Name =  userInfoModelResponseObj.Name;
                userInfoModelResponseObj.Surname =  userInfoModelResponseObj.Surname;
                userInfoModelResponseObj.Username =  userInfoModelResponseObj.Username;
            }
            return OperationObjectResult<UserInfoModelResponse>.CreateCorrectResponseGeneric(userInfoModelResponseObj);
            
         }

        
    }
}