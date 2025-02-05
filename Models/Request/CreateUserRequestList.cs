using Amazon.DAL.Models.Response;
using MediatR;

namespace Amazon.Models.Request
{
    // Classe che rappresenta una lista di richieste per la creazione di più utenti
    public class CreateUserRequestList : IRequest<UserDALResponse>
    {
        public List<CreateUserRequest> Users { get; set; } = new List<CreateUserRequest>();
    }
}
