namespace Amazon.DAL.Handlers.PasswordHasher.Abstract
{
	public interface IDevelopparePassworHasher
	{
        public string GenerateSalt();

        public string HeshPassword(string password, string base64Salt);

        public bool VerifyPassword(string password, string hashPassword, string base64Salt);
	}
}