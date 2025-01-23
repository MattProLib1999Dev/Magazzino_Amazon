using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon.Models
{
	public class UserModelResponse
	{
               
                [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public long IdUser { get; set; }

                [ForeignKey("foreignKeyProdottoUserModelResponse")]
                public int foreignKeyProdottoUserModelResponseId { get; set; }


                public string Name { get; set; } = String.Empty;


                public string Surname { get; set; } = String.Empty;


                public string Password { get; set; } = String.Empty;


                public string Username { get; set; } = String.Empty;

                public string DoubleOptInToken { get; set; } = String.Empty;


                public bool Confirmed { get; set; }


	}
}