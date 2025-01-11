using Amazon;
using Amazon.Appunti.Handlers.Abstract;
using Amazon.Common;
using Amazon.DAL.Handlers.Models.Request;
using Amazon.DAL.Models.Response;
public class FakeDatabase : IAccountDataSource
{
    private ILogger<FakeDatabase> logger;
    public List<UserDALResponse> Users = new List<UserDALResponse>();
    public static long IdAutoincrememntPrimaryKeyUser = 0;
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
        return Task.FromResult(OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(Users));
    }
    
    public async Task<OperationObjectResult<List<UserDALResponse>>> UserInfo(UserInfoHandlerRequest request)
{
    var selectedUser = Users.FirstOrDefault(x => x.IdUser.Equals(request.IdUser));
    if (selectedUser == null)
        return OperationObjectResult<List<UserDALResponse>>.CreateErrorResponse(OperationObjectResultStatus.NotFound);
    
    // Creazione di una lista contenente il singolo utente
    var userList = new List<UserDALResponse> { selectedUser };
    return OperationObjectResult<List<UserDALResponse>>.CreateCorrectResponse(userList, "");
}


    public Task<OperationObjectResult<OperationObjectResult<UserDALResponse>>> Login(LoginDALRequest request)
    {
        throw new NotImplementedException();
    }

}
