using Amazon.AccessTokenComponent.Model;
using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;

public static class AccessTokenModelMapper
{
    public static OperationObjectResult<List<UserDALResponse>> MapToUserDALResponse(OperationObjectResult<List<AccessTokenModel>> response)
    {
        if (response.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(response.Status, response.Message);
        }

        var userDALResponses = response.Value.Select(accessToken => new UserDALResponse
        {
            IdUser = accessToken.IdUser,
            Username = accessToken.UserName
        }).ToList();

        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(userDALResponses);
    }

    public static OperationObjectResult<List<AccessTokenModel>> MapToAccessTokenModel(OperationObjectResult<List<AccessTokenModel>> response)
    {
        // Controlla se la risposta ha esito positivo (Ok)
        if (response.Status != OperationObjectResultStatus.Ok)
        {
            // Se la risposta non è Ok, restituisci un errore
            return OperationObjectResult<List<AccessTokenModel>>.CreateErrorResponse(response.Status, response.Message);
        }

            // Mappa la risposta da UserHandlerResponse a AccessTokenModel
             var userDALResponse = response.Value.Select(accessToken => new AccessTokenModel
            {
                IdUser = accessToken.IdUser,
                AccesstokemModel = accessToken.AccesstokemModel
            }).ToList();

        // Restituisci la risposta con l'AccessTokenModel mappato
        return OperationObjectResult<List<AccessTokenModel>>.CreateCorrectResponseGeneric(userDALResponse);
    }



    public static OperationObjectResult<UserDALResponse> MapToAccessTokenModelSingleObj(OperationObjectResult<UserDALResponse> response)
    {
        if (response.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<UserDALResponse>.CreateErrorResponse(response.Status, response.Message);
        }

        // Creazione di un singolo oggetto UserDALResponse
        var userDALResponse = new UserDALResponse
        {
            IdUser = response.Value.IdUser,
            Name = response.Value.Name,
            Surname = response.Value.Surname,
            Password = response.Value.Password,
            ErrorMessage = response.Value.ErrorMessage,
            ErrorCode = response.Value.ErrorCode
        };

        // Restituisci un oggetto OperationObjectResult<UserDALResponse> con il valore appena creato
        return OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(userDALResponse);
    }



    public static OperationObjectResult<List<UserHandlerResponse>> MapFromUsersDalResponse(OperationObjectResult<UserDALResponse> response)
    {
        if (response.Status != OperationObjectResultStatus.Ok)
            return OperationObjectResult<List<UserHandlerResponse>>.CreateErrorResponse(response.Status, response.Message);

        var userHandlerResponseList = new List<UserHandlerResponse>
        {
            new UserHandlerResponse
            {
                IdUser = response.Value.IdUser,
                Name = response.Value.Name,
                Surname = response.Value.Surname,
                Username = response.Value.Username,
                Password = response.Value.Password
            }
        };

        return OperationObjectResult<List<UserHandlerResponse>>.CreateCorrectResponseGeneric(userHandlerResponseList);
    }

    public static OperationObjectResult<UserHandlerResponse> MapFromUsersResponseForAccessToken(OperationObjectResult<UserDALResponse> response)
    {
        if (response.Status != OperationObjectResultStatus.Ok)
            return OperationObjectResult<UserHandlerResponse>.CreateErrorResponse(response.Status, response.Message);
        return OperationObjectResult<UserHandlerResponse>.CreateCorrectResponseGeneric(new UserHandlerResponse
        {
            IdUser = response.Value.IdUser,
            Name = response.Value.Name,
            Surname = response.Value.Surname,
            Username = response.Value.Username,
            Password = response.Value.Password
        });

    }

    public static OperationObjectResult<RefreshTokenModelRequest> MapFromTokenHandlersRequest(TokenHandlerRequest request)
    {
        return OperationObjectResult<RefreshTokenModelRequest>.CreateCorrectResponseGeneric(new RefreshTokenModelRequest { AccessToken = request.AccessToken, RefreshToken = request.RefreshToken});
    }

}


