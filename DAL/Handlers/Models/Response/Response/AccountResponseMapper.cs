using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;
using Amazon.DAL.Models.Response;
using Amazon.Models;
using Amazon.Models.Request;
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

        public static OperationObjectResult<TokenModelResponse> MapFromCreateUsersHandlerResponse(OperationObjectResult<List<UserDALResponse>> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok || response.Value == null || !response.Value.Any())
                return OperationObjectResult<TokenModelResponse>.CreateErrorResponse(response.Status, response.Message);

            var userDALResponse = response.Value.First();  // Assuming you want the first user

            return OperationObjectResult<TokenModelResponse>.CreateCorrectResponseGeneric(new TokenModelResponse
            {
                AccessToken = userDALResponse.AccessToken,
                RefreshToken = userDALResponse.RefreshToken
            });
        }


        public static OperationObjectResult<List<UserModelResponse>> MapFromUsersHandler(OperationObjectResult<List<UserHandlerResponse>> users)
        {
            if (users.Status != OperationObjectResultStatus.Ok)
                return OperationObjectResult<List<UserModelResponse>>.CreateErrorResponse(users.Status, users.Message);
            var result = new List<UserModelResponse>();
            users.Value.ForEach(u => result.Add(
                new UserModelResponse
                {
                    IdUser = u.IdUser,
                    Name = u.Name,
                    Surname = u.Surname,
                    Username = u.Username,
                    Password = u.Password,
                    Confirmed = u.Confirmed
                }
            ));
            return OperationObjectResult<List<UserModelResponse>>.CreateCorrectResponseGeneric(result);
        }
    }
}