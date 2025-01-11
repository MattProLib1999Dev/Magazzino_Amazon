using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.Models.Response.Response;
using Amazon.DAL.Models.Response;

namespace Amazon.Handlers.Abstratc
{
  public class AccountHandler
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

    
  }
}
