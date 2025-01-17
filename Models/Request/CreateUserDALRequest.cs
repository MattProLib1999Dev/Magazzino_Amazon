namespace Amazon.Models.Request
{
	public class CreateUserDALRequest
	{
                public string Name { get; set; } = String.Empty;
                public string Surname { get; set; } = String.Empty;
                public string Username { get; set; } = String.Empty;
                public string Password { get; set; } = String.Empty;
	}
}