using Amazon.Common;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;
using Amazon.Models.Response;

namespace Amazon.DAL.Handlers.Models.Response.Mappers
{
    public class AccountHandlerResponseMapper
    {
        public static OperationObjectResult<List<UserDALResponse>> MapFromUsersDALResponse(OperationObjectResult<List<UserDALResponse>> users)
        {
            if (users.Status != OperationObjectResultStatus.Ok)
            {
                // Restituisci un risultato di errore mantenendo il tipo OperationObjectResult
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

            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(result);
        }


        public static OperationObjectResult<List<UserDALResponse>> MapFromUserInfoHandlerResponse(OperationObjectResult<UserInfoHandlerResponse> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(response.Status, response.Message);
            }

            var result = new List<UserDALResponse>
            {
                new UserDALResponse
                {
                    IdUser = response.Value.IdUser,
                    Name = response.Value.Name,
                    Surname = response.Value.Surname,
                    Username = response.Value.Username,
                    Password = String.Empty 
                }
            };

            return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(result);
        }

        public static List<UserDALResponse> MapFromUserResponseForAccessToken(OperationObjectResult<UserDALResponse> response)
        {
            if (response.Status != OperationObjectResultStatus.Ok)
                return null; 

            var userHandlerResponse = new UserHandlerResponse
            {
                IdUser = (int)response.Value.IdUser,
                Name = response.Value.Name,
                Surname = response.Value.Surname,
                Password = response.Value.Password,
                Users = new List<UserDALResponse> { response.Value }  
            };

            return userHandlerResponse.Users;  
        }







    }

}