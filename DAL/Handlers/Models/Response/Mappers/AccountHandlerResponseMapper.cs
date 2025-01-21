using Amazon.AccessTokenComponent.Model;
using Amazon.Common;
using Amazon.DAL.Models.Response;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.Models.Response;
using Amazon.DoubleOptInComponent.Models.Request.Response;
using System.Collections.Generic;

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


        public static OperationObjectResult<TokenHandlerResponse> MapFromAccessTokenEncriptModel(OperationObjectResult<AccessTokenEncriptModelResponse> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok)
                return OperationObjectResult<TokenHandlerResponse>.CreateErrorResponse(response.Status, response.Message);
            return OperationObjectResult<TokenHandlerResponse>.CreateCorrectResponseGeneric(new TokenHandlerResponse
            {
                AccessToken = response.Value.Accesstoken,
                RefreshToken = response.Value.RefeshToken
            });
        }

        public static OperationObjectResult<List<AccessTokenModel>> MapToUserDALResponse(OperationObjectResult<List<UserDALResponse>> loginHandlerResponse)
{
    if (loginHandlerResponse.Status != OperationObjectResultStatus.Ok)
    {
        return OperationObjectResult<List<AccessTokenModel>>.CreateErrorResponse(loginHandlerResponse.Status, loginHandlerResponse.Message);
    }

    var result = new List<AccessTokenModel>();

    // Assuming loginHandlerResponse.Value is a List<UserDALResponse>
    foreach (var user in loginHandlerResponse.Value)
    {
        result.Add(new AccessTokenModel
        {
            IdUser = user.IdUser,
            UserName = user.Username,
            AccesstokemModel = user.AccesstokenModel // Ensure this property exists in UserDALResponse
        });
    }

    return OperationObjectResult<List<AccessTokenModel>>.CreateCorrectResponseGeneric(result);
}


        public static AccessTokenModel MapToAccessTokenModel(UserHandlerResponse userHandlerResponse)
        {
            return new AccessTokenModel
            {
                IdUser = userHandlerResponse.IdUser,
                UserName = userHandlerResponse.Username
            };
        }

        public static OperationObjectResult<List<AccessTokenModel>> MapFromUserResponseForAccessToken(OperationObjectResult<List<UserDALResponse>> response)
        {
            // Controlla se la risposta ha esito positivo (Ok)
            if (response.Status != OperationObjectResultStatus.Ok)
            {
                // Se la risposta non è Ok, restituisci un errore
                return OperationObjectResult<List<AccessTokenModel>>.CreateErrorResponse(response.Status, response.Message);
            }

            // Mappa la risposta da UserDALResponse a UserHandlerResponse
            var result = new List<AccessTokenModel>();
            response.Value.ForEach(x => result.Add(new AccessTokenModel
            {
                IdUser = x.IdUser,
                UserName = x.Username,
                AccesstokemModel = x.AccesstokenModel,

            }));

            return OperationObjectResult<List<AccessTokenModel>>.CreateCorrectResponseGeneric(result);
        }

        public static  OperationObjectResult<List<UserDALResponse>> MapFromCreateUsersHandlerResponse(OperationObjectResult<List<UserDALResponse>> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok)
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(response.Status, response.Message);
            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(new List<UserDALResponse>());
        }

        public static OperationObjectResult<List<UserDALResponse>> MapFromDoubleOptInResponse(OperationObjectResult<List<DoubleOptInModelResponse>> doubleOptInTokenResult)
{
    // Verifica che lo stato dell'operazione sia Ok
    if (doubleOptInTokenResult.Status != OperationObjectResultStatus.Ok)
    {
        // Se c'è un errore, restituiamo un errore per il mapping
        return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(
            doubleOptInTokenResult.Status,
            doubleOptInTokenResult.Message
        );
    }

    // Mappiamo ogni elemento della lista in un UserDALResponse
    var userDALResponses = doubleOptInTokenResult.Value.Select(doubleOptInModel => new UserDALResponse
    {
        AccesstokenModel = doubleOptInModel.AccesstokenModel,
        AccountSecuritySalt = doubleOptInModel.AccountSecuritySalt,
        IdUser = doubleOptInModel.IdUser,
        Name = doubleOptInModel.Name,
        Surname = doubleOptInModel.Surname,
        Password = doubleOptInModel.Password
    }).ToList();

    // Restituiamo il risultato con la lista mappata
    return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(userDALResponses);
}



        private static OperationObjectResultStatus MapUserStatusToOperationObjectResultStatus(UserStatus userStatus)
        {
            return userStatus switch
            {
                UserStatus.Ok => OperationObjectResultStatus.Ok,
                UserStatus.Error => OperationObjectResultStatus.Error,
                UserStatus.TemporaryRedirect => OperationObjectResultStatus.TemporaryRedirect,
                UserStatus.NotFound => OperationObjectResultStatus.NotFound,
            };
        }

public static OperationObjectResult<ConfirmUserHandlerResponse> MapUserResponseConfirmUser(OperationObjectResult<UserDALResponse> response)
{
    if (response.Status != OperationObjectResultStatus.Ok)
        return OperationObjectResult<ConfirmUserHandlerResponse>.CreateErrorResponse(response.Status, response.Message);
    return OperationObjectResult<ConfirmUserHandlerResponse>.CreateCorrectResponseGeneric(new ConfirmUserHandlerResponse());
}  


    }
}
