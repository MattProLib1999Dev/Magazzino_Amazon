using Amazon;
using Amazon.AccessTokenComponent.Model.Abstract;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Handlers.Models.Response;
using Amazon.DAL.Handlers.Models.Response.Mappers;
using Amazon.DAL.Handlers.PasswordHasher.Abstract;
using Amazon.DAL.Models.Response;
using Amazon.DoubleOptInComponent.Abastract;
using Amazon.Handlers.Abstract;
using Amazon.Handlers.Abstratc.Mappers;
using Amazon.Models.Request;
public class FakeDatabase : IAccountDataSource
{
    private ILogger<FakeDatabase> logger;
    public List<UserDALResponse> Users = new List<UserDALResponse>();
    public long IdAutoincrememntPrimaryKeyUser = 0;
    public readonly List<UserDALResponse>? inputLogger;
    public readonly IDevelopparePassworHasher _passwordHasher;
    public readonly IDoubleOptInManager _doubleOptInManager;
    public FakeDatabase(ILogger<FakeDatabase> inputLogger, IDevelopparePassworHasher passworHasher)
    {
        logger = inputLogger;
        Users = new List<UserDALResponse>();
        _passwordHasher = passworHasher;
        AddFakeUsers();
    }

    public List<AddProdotto> ListProdotto { get; set; } = new List<AddProdotto>();

    public List<AddProdottoDtoInput> ListaDiTuttiIProdotti = new List<AddProdottoDtoInput>();

    public InviaUnProdottoDto InviaUnProdottoDto = new InviaUnProdottoDto();

    public CreaProdottoInputDto CreaUnProdotto = new CreaProdottoInputDto();

    public UpdateProdottoDtoInput ModificaUnProdotto = new UpdateProdottoDtoInput();

    public List<ProdottoEntity> prodotto { get; set; } = new List<ProdottoEntity>();
    private readonly ILogger<AccountHandler> _logger;

    public bool prodottoCancellato = new bool();
    private IAccessTokenManager _accountDataSource;

    public void AddUsersInternal(UserDALResponse user)
    {
        IdAutoincrememntPrimaryKeyUser++;
        user.IdUser = IdAutoincrememntPrimaryKeyUser;
        user.OriginalPassword = user.Password;
        user.PasswordSecuritySalt = _passwordHasher.GenerateSalt();
        user.Password = _passwordHasher.HeshPassword(user.Password, user.PasswordSecuritySalt);
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

        if (_passwordHasher.VerifyPassword(request.Password,user.Password, user.PasswordSecuritySalt))
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
        logger.LogInformation("Starting CreateUser with Username: {Username}", request.Username);

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
            logger.LogInformation("User created successfully: {Username}", request.Username);
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(response));
        }
        else if (user.Status == UserStatus.Created)
        {
            logger.LogWarning("User already exists: {Username}", request.Username);
            user.AccountSecuritySalt = Guid.NewGuid().ToString("N");
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Conflict, "User Already Exists"));
        }

        // Handle other statuses or unexpected cases
        logger.LogError("Unhandled user status: {Status} for Username: {Username}", user.Status, request.Username);
        return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest, $"Bad request User Status: {user.Status}"));
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating user: {Username}", request.Username);
        return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Error, ex.Message));
    }
}


    public Task<OperationObjectResult<UserDALResponse>> CreateUser(List<CreateUserHandlerRequest> request)
    {
        throw new NotImplementedException();
    }

    public Task<OperationObjectResult<List<UserDALResponse>>> Login(List<LoginHandlerRequest> request)
    {
        throw new NotImplementedException();
    }

    public Task<OperationObjectResult<UserDALResponse>> ConfirmUser(ConfirmCreateUserDALRequest request)
    {
        var user = Users.FirstOrDefault(u => u.NeedConfirm()
            && u.Username.Equals(request.Username, StringComparison.InvariantCulture)
            && u.IdUser == request.IdUser
            && request.DoubleOptInToken.Equals(u.AccountSecuritySalt, StringComparison.InvariantCulture));
        if (user == null)
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.NotFound));
        user.Status = UserStatus.Confirmed;
        return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(user));
    }

        public async Task<OperationObjectResult<ConfirmUserHandlerResponse>> ConfirmUser(ConfirmUserHandlerRequest request)
        {
            try
            {
                // Map the confirmation request
                var mappedRequest = DoubleOptInRequestMapper.MatToDoubleOptInRequest(request);

                // Verify the double opt-in token
                var verifyResult = await _doubleOptInManager.VerifyDoubleOptInToken(mappedRequest);

                // Check if verification was successful
                if (verifyResult.Status != OperationObjectResultStatus.Ok)
                {
                    return OperationObjectResult<ConfirmUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.BadRequest);
                }

                // Map the verification result to a list of UserDALResponse
                var dalRequest = verifyResult.Value.Select(optIn => new ConfirmCreateUserDALRequest 
                {
                    IdUser = optIn.IdUser,
                    Username = optIn.Username,
                    // Map other properties here as necessary
                }).ToList();


                // Call to confirm the user
                var result = await _accountDataSource.ConfirmUser(dalRequest);

                // Map the confirmed user response
                return AccountHandlerResponseMapper.MapUserResponseConfirmUser(result);
            }
            catch (Exception ex)
            {
                // Log errors and return error response with the message
                _logger.LogError(ex, "Error during user confirmation process");
                return OperationObjectResult<ConfirmUserHandlerResponse>.CreateErrorResponse(OperationObjectResultStatus.Ok, ex.Message);
            }
        }

    public Task<OperationObjectResult<ConfirmUserHandlerResponse>> ConfirmUser(List<ConfirmUserHandlerRequest> request)
    {
        throw new NotImplementedException();
    }
}
