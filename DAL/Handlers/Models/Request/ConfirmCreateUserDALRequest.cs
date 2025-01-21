namespace Amazon.DAL.Handlers.Models.Request
{
	public class ConfirmCreateUserDALRequest
	{
        public long IdUser { get; set; }

        public string Username { get; set; } = String.Empty;

        public string DoubleOptInToken { get; set; } = String.Empty;
	}
}