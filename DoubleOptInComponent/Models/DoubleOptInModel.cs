namespace Amazon.DoubleOptInComponent.Models
{
	public class DoubleOptInModel
	{
        public long IdUser { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string DoubleOptInToken { get; set; } = String.Empty;

	}
}