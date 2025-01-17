using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Amazon.DAL.Handlers.PasswordHasher.Abstract
{
    public class PBKDF2_PasswordHasher : IDevelopparePassworHasher
    {
        public string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        }

        public string HeshPassword(string password, string base64Salt)
        {
            return HeshPasswordInternal(password, base64Salt);
        }

        public string MyProperty(string password, string base64Salt)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPassword(string password, string hashPassword, string base64Salt)
        {
            return hashPassword.Equals(HeshPasswordInternal(password, base64Salt), StringComparison.InvariantCulture);
        }

        private string HeshPasswordInternal(string password, string base64Salt)
		{
			var saltByte = Convert.FromBase64String(base64Salt);
			byte[] hashBytes = KeyDerivation.Pbkdf2(
				password: password!,
				salt: saltByte,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 100000,
				numBytesRequested: 256 / 8
			);

			// Convert the byte array to a base64 string
			return Convert.ToBase64String(hashBytes);
		}

    }
}