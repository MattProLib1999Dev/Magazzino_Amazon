using Amazon.AccessTokenComponent.Model.Request;
using Amazon.Common;
using Amazon.Models.Response;

namespace Amazon.DAL.Handlers.Models.Response.Response
{
    public class AccountResponseMapper
    {
        public static OperationObjectResult<UserInfoModelResponse> MapFromLoginHandlerResponse(OperationObjectResult<LoginHandlerResponse> response)
        {
            // Controllo dello stato della risposta
            if (response.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<UserInfoModelResponse>.CreateErrorResponse(response.Status, response.Message);
            }

            // Controllo del valore della risposta
            if (response.Value == null)
            {
                return OperationObjectResult<UserInfoModelResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest, "Il valore della risposta è null.");
            }

            // Creazione della risposta mappata
            var responseValue = response.Value;
            var userInfoModelResponse = new UserInfoModelResponse
            {
                IdUser = responseValue.IdUser,
                Name = responseValue.Name,
                Surname = responseValue.Surname,
                Username = responseValue.Username
            };

            return OperationObjectResult<UserInfoModelResponse>.CreateCorrectResponseGeneric(userInfoModelResponse);
        }

        public static OperationObjectResult<TokenModelResponse> MapFromCreateUsersDALResponse(
            OperationObjectResult<TokenHandlerResponse> response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "Il parametro response non può essere null.");
            }

            // Gestione degli errori: Se lo stato non è OK, restituisce un errore
            if (response.Status != OperationObjectResultStatus.Ok)
            {
                return OperationObjectResult<TokenModelResponse>.CreateErrorResponse(
                    response.Status,
                    response.Message ?? "Errore non specificato durante la creazione dell'utente."
                );
            }

            // Mappatura della risposta valida
            if (response.Value == null)
            {
                return OperationObjectResult<TokenModelResponse>.CreateErrorResponse(
                    OperationObjectResultStatus.Error,
                    "La risposta contiene uno stato OK ma il valore è null."
                );
            }

            // Creazione della risposta mappata
            var mappedResponse = new TokenModelResponse
            {
                AccessToken = response.Value.AccessToken,
                RefreshToken = response.Value.RefreshToken
                // Aggiungi altre proprietà se necessario
            };

            return OperationObjectResult<TokenModelResponse>.CreateCorrectResponseGeneric(mappedResponse);
        }

    }
}