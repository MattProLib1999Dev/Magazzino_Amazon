using Amazon.AccessTokenComponent.Model;
using Amazon.Common;
using Amazon.DAL.Models.Response;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.Models.Response;
using LoginHandlerResponse = Amazon.DAL.Handlers.Models.Response.Response.LoginHandlerResponse;

namespace Amazon.DAL.Handlers.Models.Response.Mappers
{
    public class AccountHandlerResponseMapper
    {
        public static OperationObjectResult<List<UserDALResponse>> MapFromUsersDALResponse(OperationObjectResult<List<UserDALResponse>> users)
        {
            if (users.Status != OperationObjectResultStatus.Ok)
            {
                // Return error result, keeping the OperationObjectResult type
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(users.Status, users.Message);
            }

            var result = new List<UserDALResponse>();
            users.Value.ForEach(x => result.Add(new UserDALResponse
            {
                IdUser = x.IdUser,
                Name = x.Name,
                Surname = x.Surname,
                Username = x.Username,
                Password = x.Password
            }));

            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(result);
        }

        public static OperationObjectResult<List<UserDALResponse>> ConvertResponse(OperationObjectResult<UserInfoHandlerResponse> input)
        {
            if (input.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(input.Status, input.Message);
            }

            var userDalResponseList = new List<UserDALResponse>
            {
                new UserDALResponse
                {
                    IdUser = input.Value.IdUser,
                    Name = input.Value.Name,
                    Surname = input.Value.Surname,
                    Username = input.Value.Username,
                    Password = string.Empty // Evitiamo di trasferire la password
                }
            };

            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(userDalResponseList);
        }


        public static OperationObjectResult<LoginHandlerResponse> MapFromAccessTokenEncriptModel(OperationObjectResult<AccessTokenEncriptModel> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(response.Status, response.Message);
            }

            var loginHandlerResponse = new LoginHandlerResponse
            {
                AccessToken = response.Value.Accesstoken,
                IdUser = response.Value.IdUser
            };

            // Creazione della risposta con il loginHandlerResponse
            return OperationObjectResult<LoginHandlerResponse>.CreateCorrectResponseGeneric(loginHandlerResponse);
        }

        public static UserDALResponse MapToUserDALResponse(Response.LoginHandlerResponse loginHandlerResponse)
        {
            return new UserDALResponse
            {
                IdUser = loginHandlerResponse.IdUser,
                Name = loginHandlerResponse.Name,
                Surname = loginHandlerResponse.Surname,
                Username = loginHandlerResponse.Username,
                Password = loginHandlerResponse.Password ?? string.Empty
            };
        }

        public static AccessTokenModel MapToAccessTokenModel(UserHandlerResponse userHandlerResponse)
        {
            return new AccessTokenModel
            {
                IdUser = userHandlerResponse.IdUser,
                UserName = userHandlerResponse.Username
            };
        }

        public static OperationObjectResult<UserHandlerResponse> MapFromUserResponseForAccessToken(OperationObjectResult<UserDALResponse> response)
        {
            // Controlla se la risposta ha esito positivo (Ok)
            if (response.Status != OperationObjectResultStatus.Ok)
            {
                // Se la risposta non è Ok, restituisci un errore
                return OperationObjectResult<UserHandlerResponse>.CreateErrorResponse(response.Status, response.Message);
            }

            // Mappa la risposta da UserDALResponse a UserHandlerResponse
            var userHandlerResponse = new UserHandlerResponse
            {
                IdUser = response.Value.IdUser,
                Name = response.Value.Name,
                Surname = response.Value.Surname,
                Username = response.Value.Username,
                Password = response.Value.Password // Nota: gestire la password in modo sicuro è importante
            };

            // Restituisci un'OperationObjectResult con un oggetto UserHandlerResponse mappato
            return OperationObjectResult<UserHandlerResponse>.CreateCorrectResponseGeneric(userHandlerResponse);
        }

       


    }
}
