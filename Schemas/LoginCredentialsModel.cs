using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class LoginCredentialsModel
    {
        [Required][EmailAddress] public string email { get; set; }
        [Required] public string password { get; set; }
    }
}
