namespace Amazon.DAL.Handlers.Models.Request
{
	public class ConfirmUserHandlerRequest
	{
        public string DoubleOptInToken { get; set; } = String.Empty;
		public long IdUser { get; set; }
		public string Username { get; set; } = String.Empty;
	}
}