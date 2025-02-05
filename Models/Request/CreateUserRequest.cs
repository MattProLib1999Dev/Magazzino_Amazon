using Amazon.DAL.Models.Response;
using MediatR;

namespace Amazon.Models.Request
{
    // La richiesta che contiene i dati per creare un nuovo utente
    public class CreateUserRequest : IRequest<UserDALResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
