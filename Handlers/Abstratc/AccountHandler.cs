using Amazon.AccessTokenComponent.Model;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Amazon.Models.Response;

namespace Amazon.Handlers.Abstratc
{
  public class AccountHandler : IAccountHandler
  {
    public readonly ILogger<AccountHandler> logger;
    public readonly IAccountDataSource accountDataSource;
    public readonly IAccessTokenManager accessTokenManager;
    public AccountHandler(IAccountDataSource inputAccountDataSource, ILogger<AccountHandler> inputLogger, IAccessTokenManager inputAccessTokenManager)
    {
      accountDataSource = inputAccountDataSource;
      logger = inputLogger;
      accessTokenManager = inputAccessTokenManager;
    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers()
    {
      try
      {
        var dalResult = await accountDataSource.GetAllUsers();
        var resultMapped = AccountHandlerResponseMapper.MapFromUsersDALResponse(dalResult);
        return resultMapped;
      }
      catch (Exception ex)
      {

        logger.LogError(ex.Message);
        var users = new List<UserDALResponse>();

        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(users, "Operation succeeded");
      }
    }

    public async Task<OperationObjectResult<List<UserDALResponse>>> UserInfo(UserInfoHandlerRequest request)
    {
      try
      {
        var dalResult = await accountDataSource.UserInfo(request);

        if (dalResult.Status != OperationObjectResultStatus.Ok)
        {
          return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(dalResult.Status, dalResult.Message);
        }

        // Converte il singolo oggetto in una lista
        var usersList = new List<UserDALResponse> { dalResult.Value };

        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(usersList);
      }
      catch (Exception ex)
      {
        logger.LogError(ex.Message);

        var users = new List<UserDALResponse>();
        return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(users, "Operation succeeded");
      }
    }




    public async Task<OperationObjectResult<LoginHandlerResponse>> Login(LoginHandlerRequest request)
    {   
          try
          {
              var mappedRequest = AccountHandlerRequestMapper.MapToLoginRequest(request);

              // Esegui il login, che restituisco una lista di utenti
              var userResult = await accountDataSource.Login(mappedRequest);

              // Se la risposta non è OK o non ci sono utenti, restituisco un errore
              if (userResult.Status != OperationObjectResultStatus.Ok || userResult.Value == null || !userResult.Value.Any())
              {
                  return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(userResult.Status, "No users found.");
              }

              // Se ci sono utenti, prendi il primo utente (dato che stai cercando un singolo utente)
              var singleUser = await accountDataSource.Login(mappedRequest);

              var userHandlerResponse = AccountHandlerResponseMapper.MapFromUsersDALResponse(singleUser);  

              var requestAccessToken = AccessTokenModelMapper.MapToAccessTokenModel(userHandlerResponse);

              // Se l'utente non esiste, restituisco un errore
              if (singleUser == null)
              {
                  return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, "User not found.");
              }  

              // Crea una lista di AccessTokenModel (da passare alla generazione del token)
               var accessTokenList = new List<AccessTokenModel>();



              // Ora genera il token per l'accesso
              var tokenGenerationResult = await accessTokenManager.GenerateToken(accessTokenList);

              // Se il risultato della generazione del token non è valido, restituisci un errore
              if (tokenGenerationResult == null || tokenGenerationResult.Value == null)
              {
                  return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(
                      OperationObjectResultStatus.Error, 
                      "Failed to generate access token.");
              }

              // Mappa il risultato finale (prendi il primo access token criptato)
              var accessTokenEncriptModel = tokenGenerationResult.Value;

              // Restituisco il risultato mappato
              tokenGenerationResult.Value.FirstOrDefault();

          }
          catch (Exception ex)
          {
              // Log dell'errore
              logger.LogError(ex, "An error occurred during the login process.");
              
              // Restituisci un errore generico
              return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(
                  OperationObjectResultStatus.Error, 
                  ex.Message);
          }

          return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(
            OperationObjectResultStatus.Error, 
            "error message");

          
}








  }
}
