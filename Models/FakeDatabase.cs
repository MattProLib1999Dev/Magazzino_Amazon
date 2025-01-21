using Amazon;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.PasswordHasher.Abstract;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Azure;
public class FakeDatabase : IAccountDataSource
{
    private ILogger<FakeDatabase> logger;
    public List<UserDALResponse> Users = new List<UserDALResponse>();
    public long IdAutoincrememntPrimaryKeyUser = 0;
    public readonly List<UserDALResponse>? inputLogger;
    public readonly IDevelopparePassworHasher passwordHasher;
    public FakeDatabase(ILogger<FakeDatabase> inputLogger)
    {
        logger = inputLogger;
        Users = new List<UserDALResponse>();
        AddFakeUsers();
    }

    public List<AddProdotto> ListProdotto { get; set; } = new List<AddProdotto>();

    public List<AddProdottoDtoInput> ListaDiTuttiIProdotti = new List<AddProdottoDtoInput>();

    public InviaUnProdottoDto InviaUnProdottoDto = new InviaUnProdottoDto();

    public CreaProdottoInputDto CreaUnProdotto = new CreaProdottoInputDto();

    public UpdateProdottoDtoInput ModificaUnProdotto = new UpdateProdottoDtoInput();

    public List<ProdottoEntity> prodotto { get; set; } = new List<ProdottoEntity>();

    public bool prodottoCancellato = new bool();

    public void AddUsersInternal(UserDALResponse user)
    {
        IdAutoincrememntPrimaryKeyUser++;
        user.IdUser = IdAutoincrememntPrimaryKeyUser;
        user.OriginalPassword = user.Password;
        user.PasswordSecuritySalt = passwordHasher.GenerateSalt();
        user.Password = passwordHasher.HeshPassword(user.Password, user.PasswordSecuritySalt);
        user.AccountSecuritySalt = Guid.NewGuid().ToString("N");
    }

    public void AddFakeUsers()
    {
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1", Status = UserStatus.Confirmed });

    }

    public Task<OperationObjectResult<List<UserDALResponse>>> GetAllUsers()
    {
        return Task.FromResult(OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponseGeneric(Users));
    }

    public async Task<OperationObjectResult<UserDALResponse>> UserInfo(UserInfoHandlerRequest request)
    {
        var selectedUser = Users.FirstOrDefault(x => x.IdUser == request.IdUser);

        if (selectedUser == null)
        {
            return OperationObjectResult<UserDALResponse>.CreateErrorResponse(
                OperationObjectResultStatus.Error,
                "User not found."
            );
        }

        var userResponse = await Task.Run(() => new UserDALResponse
        {
            IdUser = selectedUser.IdUser,
            Name = selectedUser.Name,
            Surname = selectedUser.Surname,
            Username = selectedUser.Username,
            Password = selectedUser.Password
        });

        return OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(userResponse);
    }




    public Task<OperationObjectResult<UserDALResponse>> Login(LoginDALRequest request)
{
    try
    {
        // Usa string.Equals per confrontare le stringhe in modo case-insensitive
        var user = Users.FirstOrDefault(x => x.isValid() && string.Equals(x.Username, request.Username, StringComparison.InvariantCulture));
        
        if (user == null)
        {
            // Restituisce un errore se l'utente non viene trovato
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.NotFound));
        }

        if (passwordHasher.VerifyPassword(request.Password,user.Password, user.PasswordSecuritySalt))
        {
            // Restituisce la risposta corretta con i dati dell'utente
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(user));
        }

        // Restituisce un errore se la password non è corretta
        return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Error));
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
        // Restituisce un errore generico in caso di eccezione
        return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Error));
    }
}



    public Task<OperationObjectResult<UserDALResponse>> CreateUser(CreateUserDALRequest request)
    {
        try
        {
            var user = Users.FirstOrDefault(u => u.isValid() && u.Username.Equals(request.Username, StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
            {
                var response = new UserDALResponse
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Username = request.Username,
                    Password = request.Password,
                    Status = UserStatus.Created
                };

                AddUsersInternal(response);
                return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(response));
            }
            else if (user.Status == UserStatus.Created)
            {
                user.AccountSecuritySalt = Guid.NewGuid().ToString("N");
                return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Conflict, "User Already Exists"));
            }

            // Handle other statuses or unexpected cases
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest, "Bad request User Status"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Error));
        }
    }

    public Task<OperationObjectResult<List<UserDALResponse>>> Login(List<LoginHandlerRequest> request)
    {
        throw new NotImplementedException();
    }

    Task<OperationObjectResult<List<UserDALResponse>>> IAccountDataSource.CreateUser(CreateUserDALRequest request)
    {
        throw new NotImplementedException();
    }
}
