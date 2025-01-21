using Amazon.DAL.Models.Response;

namespace Amazon.DAL.Handlers.Models.Response.Response
{
        public class UserHandlerResponse
        {
                public long IdUser { get; set; }
                public string Name { get; set; } = String.Empty;
                public string Surname { get; set; } = String.Empty;
                public string Username { get; set; } = String.Empty;
                public string Password { get; set; } = String.Empty;
                public List<UserDALResponse> Users { get; set; } 
                public string AccountSecuritySalt { get; set; } = String.Empty;
                public string AccesstokemModel { get; set; } = String.Empty;

                public bool Confirmed { get; set; }
        }

}