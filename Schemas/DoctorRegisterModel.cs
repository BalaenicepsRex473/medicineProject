using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class DoctorRegisterModel
    {
        [Required][MinLength(1), MaxLength(1000)]public string name { get; set; }
        [Required][MinLength(6)]public string password { get; set; }
        [Required][MinLength(1)][EmailAddress] public string email { get; set; }
        public DateTime birthsday { get; set; }
        [Required]public Gender gender { get; set; }

        [Phone]public string phone { get; set; }

        [Required]public Guid speciality { get; set; }
    }
}
