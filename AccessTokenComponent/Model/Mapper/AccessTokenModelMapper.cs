using Amazon.AccessTokenComponent.Model;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request; // Assumo che UserDALResponse sia definito qui

public static class AccessTokenModelMapper
{
    public static OperationObjectResult<List<UserDALResponse>> MapToUserDALResponse(OperationObjectResult<List<AccessTokenModel>> response)
    {
        if (response.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(response.Status, response.Message);
        }

        // Mappa ogni AccessTokenModel a UserDALResponse
        var userDALResponses = response.Value.Select(accessToken => new UserDALResponse
        {
            IdUser = accessToken.IdUser,
            Username = accessToken.UserName
        }).ToList();

        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(userDALResponses);
    }

    public static Task<OperationObjectResult<UserDALResponse>> MapToLoginRequest(LoginHandlerRequest request)
    {
        var accessTokenEncryptModels = new List<UserDALResponse>
        {
            new UserDALResponse
            {
                Username = request.Username,
                Password = request.Password
            }
        };

        var result = new OperationObjectResult<UserDALResponse>
        {
            Status = OperationObjectResultStatus.Ok,  
            Value = accessTokenEncryptModels.FirstOrDefault()
        };

        return Task.FromResult(result);
    }

    public static OperationObjectResult<List<UserDALResponse>> MapToAccessTokenModel(OperationObjectResult<List<UserDALResponse>> response)
    {
        if (response.Status != OperationObjectResultStatus.Ok)
        {
            return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(response.Status, response.Message);
        }

        UserDALResponse userDALResponse = new UserDALResponse();
        long IdUser = userDALResponse.IdUser;
        string UserName = userDALResponse.Username;

        var userDALResponseList = new List<UserDALResponse>
        {
            new UserDALResponse
            {
                IdUser = IdUser,
                Username = UserName
            }
        };
        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(userDALResponseList);
    }




    
}


