using Amazon;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Models.Response;
using Amazon.Models.Request;
using Azure;
public class FakeDatabase : IAccountDataSource
{
    private ILogger<FakeDatabase> logger;
    public List<UserDALResponse> Users = new List<UserDALResponse>();
    public long IdAutoincrememntPrimaryKeyUser = 0;
    public readonly List<UserDALResponse>? inputLogger;

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
        Users.Add(user);
    }

    public void AddFakeUsers()
    {
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });
        AddUsersInternal(new UserDALResponse { Name = "Name1", Surname = "Surnmame1", Username = "matt1", Password = "pass1" });

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
        var user = Users.FirstOrDefault(x =>
            x.Username.Equals(request.Username, StringComparison.InvariantCultureIgnoreCase) &&
            x.Password.Equals(request.Password));

        if (user == null)
        {
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(
                OperationObjectResultStatus.Error,
                "Invalid username or password."
            ));
        }

        return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseSingleObj(
            user,
            "Login successful."
        ));
    }

    public Task<OperationObjectResult<UserDALResponse>> CreateUser(CreateUserDALRequest request)
    {
        try
        {
            var alreadyExist = Users.Any(x => x.Username.Equals(request.Username, StringComparison.InvariantCultureIgnoreCase));
            if (alreadyExist)
                return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Conflict, "User Already Exists"));
                var response = new UserDALResponse { Name = request.Name, Surname = request.Surname, Username = request.Username, Password = request.Password};
                AddUsersInternal(response);
                return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateCorrectResponseGeneric(response));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Task.FromResult(OperationObjectResult<UserDALResponse>.CreateErrorResponse(OperationObjectResultStatus.Error));
        }
    }
}
