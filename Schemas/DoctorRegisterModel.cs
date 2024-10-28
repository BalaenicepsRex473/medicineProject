using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class DoctorRegisterModel
    {
        [Required]public string name { get; set; }
        [Required]public string password { get; set; }
        [Required][EmailAddress] public string email { get; set; }
        public DateTime birthsday { get; set; }
        [Required]public Gender gender { get; set; }

        [Phone]public string phone { get; set; }

        [Required]public Guid speciality { get; set; }
    }
}
