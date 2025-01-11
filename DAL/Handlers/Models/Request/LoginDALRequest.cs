namespace Amazon.DAL.Handlers.Models.Request
{
	public class LoginDALRequest
	{
        public string Username { get; set; } = String.Empty;

        public string Password { get; set; } = String.Empty;
	}
}