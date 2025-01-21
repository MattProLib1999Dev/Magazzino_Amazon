using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Models.Response;
using Amazon.Models;
using Amazon.Models.Request;
using Amazon.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace Amazon.DAL.Handlers.Models.Response.Mappers
{
    public static class AccountRequestMapper
    {
        public static UserInfoHandlerRequest MapToUserInfoRequest(UserInfoHandlerRequest request)
        {
            return new UserInfoHandlerRequest
            {
                IdUser = request.IdUser
            };
        }

        public static UserInfoHandlerResponse MapToUserInfoResponse(UserInfoHandlerRequest request)
        {
            return new UserInfoHandlerResponse
            {
                IdUser = request.IdUser
            };
        }

        public static LoginHandlerRequest MapToLoginRequest(LoginHandlerRequest request)
        {
            return new LoginHandlerRequest
            {
                Username = request.Username,
                Password = request.Password
            };
        }

        public static OperationObjectResult<List<UserDALResponse>> MapToCreateUserRequest(CreateUserModelRequest request)
        {
            var userDALResponseList = new List<UserDALResponse>
            {
                new UserDALResponse
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Username = request.Username,
                    Password = request.Password
                }
            };

            return new OperationObjectResult<List<UserDALResponse>>
            {
                Status = OperationObjectResultStatus.Ok,
                Value = userDALResponseList
            };
        }


        

    }


}