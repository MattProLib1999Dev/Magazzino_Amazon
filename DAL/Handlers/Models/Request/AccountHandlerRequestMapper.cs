using Amazon.Common;
using Amazon.DAL.Models.Response;
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

			// Creiamo l'oggetto OperationObjectResult con lo status e la lista di oggetti
			var result = new OperationObjectResult<UserDALResponse>
			{
				Status = OperationObjectResultStatus.Ok,  // Impostiamo lo status a Ok
			};

			// Restituiamo il Task che rappresenta l'operazione asincrona
			return Task.FromResult(result);
		}

	}
}