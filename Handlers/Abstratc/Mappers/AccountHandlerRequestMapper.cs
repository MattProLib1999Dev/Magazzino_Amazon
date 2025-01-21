using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Models;
using Amazon.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace Amazon.Handlers.Abstratc.Mappers
{
    public static class AccountHandlerRequestMapper
    {
        public static UserInfoDALRequest MapToUserInfoRequest(UserInfoHandlerRequest request)
        {
            return new UserInfoDALRequest
            {
                IdUser = request.IdUser
            };
        }

        public static CreateUserDALRequest MapToCreateUserRequest(CreateUserHandlerRequest request)
        {
            return new CreateUserDALRequest
            {
                Name = request.Name,
                Surname = request.Surname,
                Username = request.Username,
                Password = request.Password
            };
        }

        public static ConfirmCreateUserDALRequest MapToConfirmCreateUserDALRequest(DoubleOptInModel request)
        {
            return new ConfirmCreateUserDALRequest
            {
                DoubleOptInToken = request.DoubleOptInToken,
                Username = request.Username,
                IdUser = request.IdUser
            };
        }

        public static ConfirmUserModelRequest MapToConfirmUserRequest(ConfirmUserModelRequest request)
        {
            return new ConfirmUserModelRequest
            {
                DoubleOptInToken = request.DoubleOptInToken
            };
        }


    }
}
