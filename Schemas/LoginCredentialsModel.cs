using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class LoginCredentialsModel
    {
        [Required][EmailAddress][MinLength(1)] public string email { get; set; }
        [Required][MinLength(1)] public string password { get; set; }
    }
}
