using Amazon.AccessTokenComponent.Model;
using Amazon.Common;
using Amazon.DAL.Models.Response; // Assumo che UserDALResponse sia definito qui

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
}
