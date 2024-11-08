using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class DoctorModel
    {
        [Required]public Guid id { get; set; }
        [Required][MinLength(1)] public string name { get; set; }
        [Required][MinLength(1)] public string email { get; set; }
        public DateTime birthsday { get; set; }
        [Required] public Gender gender { get; set; }
        [Required] public DateTime creteTime { get; set; }
        public string phone { get; set; }

        [Required] public SpecialityModel Speciality { get; set; }
    }
}
