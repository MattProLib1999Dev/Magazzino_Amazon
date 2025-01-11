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
				Password = request.Password,
			};
		}
	}
}