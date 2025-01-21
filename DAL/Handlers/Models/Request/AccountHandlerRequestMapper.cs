using Amazon.Common;
using Amazon.DAL.Models.Response;
using Amazon.Models;
using Amazon.Models.Request;
using Amazon.Models.Response;

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

		public static List<LoginHandlerRequest> MapToLoginRequest(LoginHandlerRequest request)
		{
			return new List<LoginHandlerRequest>
			{
				new LoginHandlerRequest
				{
					Username = request.Username,
					Password = request.Password
				}
			};
		}

		public static List<CreateUserHandlerRequest> MapToCreateUserHandlerRequestList(List<UserDALResponse> userDALResponses)
		{
			var mappedList = userDALResponses.Select(user => new CreateUserHandlerRequest
			{
				Name = user.Name,
				Surname = user.Surname,
				Username = user.Username,
				Password = user.Password // Assumendo che ci sia una proprietà Password, sostituisci se necessario
			}).ToList();

			return mappedList;
		}


		public static OperationObjectResult<List<CreateUserHandlerResponse>> MapFromUsersDALResponse(OperationObjectResult<List<UserDALResponse>> users)
		{
			if (users.Status != OperationObjectResultStatus.Ok)
			{
				// Restituiamo un errore se lo stato non è OK
				return OperationObjectResult<List<CreateUserHandlerResponse>>.CreateErrorResponse(users.Status, users.Message);
			}

			// Se la lista è vuota, restituiamo un errore
			if (users.Value == null || !users.Value.Any())
			{
				return OperationObjectResult<List<CreateUserHandlerResponse>>.CreateErrorResponse(OperationObjectResultStatus.NotFound, "No users found.");
			}

			// Mappiamo la lista di UserDALResponse a CreateUserHandlerResponse
			var createUserHandlerResponses = users.Value.Select(user => new CreateUserHandlerResponse
			{
				IdUser = user.IdUser,
				Name = user.Name,
				Surname = user.Surname,
				Username = user.Username
			}).ToList();

			// Restituiamo la risposta con la lista mappata
			return OperationObjectResult<List<CreateUserHandlerResponse>>.CreateCorrectResponseGeneric(createUserHandlerResponses);
		}




		


	}
}