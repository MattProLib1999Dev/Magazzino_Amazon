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
    public AccountHandler(IAccountDataSource inputAccountDataSource, ILogger<AccountHandler> inputLogger)
    {
      accountDataSource = inputAccountDataSource;
      logger = inputLogger;
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
        var user = await accountDataSource.Login(mappedRequest);

        if (user.Status != OperationObjectResultStatus.Ok)
          return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(user.Status);

      }
      catch (Exception ex)
      {
        logger.LogError(ex.Message);
        return OperationObjectResult<LoginHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message);
      }


    }


  }
}
