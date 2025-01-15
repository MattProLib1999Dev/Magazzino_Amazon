namespace Amazon.DAL.Handlers.Models.Response.Response
{
	public class LoginHandlerResponse
	{
        public string AccessToken { get; set; }
		public long IdUser { get; set; }
		public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;

		public string Username { get; set; } = String.Empty;

		public string Password { get; set; } = String.Empty;


	}
}