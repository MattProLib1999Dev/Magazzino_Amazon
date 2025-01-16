using Amazon.DAL.Handlers.Models.Request;
using Amazon.Models.Request;
using Amazon.Models.Response;

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

        

    }


}