using Amazon.DAL.Models.Response;

namespace Amazon.DAL.Handlers.Models.Response.Response
{
        public class UserHandlerResponse
        {
                public int IdUser { get; set; }
                public string Name { get; set; }
                public string Surname { get; set; }
                public string Password { get; set; }
                public List<UserDALResponse> Users { get; set; } 
        }

}