using Amazon.Common;
using Amazon.DAL.Models.Response;
using Amazon.Models;
using Amazon.Models.Request;

namespace Amazon.DAL.Handlers.Models.Request
{
	public static class AccountHandlerRequestMapper
	{
        public static UserInfoHandlerRequest MapToUserInfoRequest(UserInfoHandlerRequest request)
		{
			return new UserInfoHandlerRequest
			{
				IdUser = request.IdUser
			};
		}

		public static LoginDALRequest MapToLoginRequest(LoginHandlerRequest request)
		{
			return new LoginDALRequest
			{
				Username = request.Username,
				Password = request.Password
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

	}
}