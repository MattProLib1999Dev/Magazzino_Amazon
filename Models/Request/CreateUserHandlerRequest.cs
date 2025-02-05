using Amazon.DAL.Models.Response;
using MediatR;


namespace Amazon.Models.Request
{
    // L'handler che gestisce la creazione di più utenti
    public class CreateUserHandler : IRequestHandler<CreateUserRequestList, UserDALResponse>
    {
        private readonly UserRepository _userRepository;

        public CreateUserHandler(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<UserDALResponse> Handle(CreateUserRequestList request, CancellationToken cancellationToken)
        {
            bool allCreated = true;
            List<string> failureMessages = new List<string>();

            foreach (var itemRequest in request.Users) // Ora `request.Users` è una lista
            {
                if (string.IsNullOrEmpty(itemRequest.Username) || string.IsNullOrEmpty(itemRequest.Password))
                {
                    allCreated = false;
                    failureMessages.Add($"Username and password are required for {itemRequest.Username}.");
                    continue;
                }

                // Crea un oggetto UserDALResponse
                var userDalResponse = new UserDALResponse
                {
                    Name = itemRequest.Name,
                    Surname = itemRequest.Surname,
                    Username = itemRequest.Username,
                    Password = itemRequest.Password
                };

                // Crea una lista con un solo oggetto UserDALResponse
                var userDalResponseList = new List<UserDALResponse>
                {
                    userDalResponse // Aggiungi l'oggetto userDalResponse alla lista
                };

                // Passa la lista al metodo CreateUser
                var result = _userRepository.CreateUser(userDalResponseList); // Se CreateUser è asincrono

               
            }

            if (allCreated)
            {
                return new UserDALResponse
                {
                    Success = true,
                    Message = "All users created successfully."
                };
            }
            else
            {
                return new UserDALResponse
                {
                    Success = false,
                    Message = string.Join(", ", failureMessages)
                };
            }
        }
    }
}
