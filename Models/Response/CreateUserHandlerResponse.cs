namespace Amazon.Models.Response
{
	public class CreateUserHandlerResponse
	{
		public long IdUser { get; set; }
		public string DoubleOptInToken { get; set; } = String.Empty;
		public string Username { get; set; } = String.Empty;
		public string Name { get; set; } = String.Empty;
		public string Surname { get; set; } = String.Empty;
		public string Password { get; set; } = String.Empty;

		public string AccessToken { get; set; } = String.Empty;
		public string RefreshToken { get; set; } = String.Empty;

		public string Status { get; set; } = String.Empty;

	}
}