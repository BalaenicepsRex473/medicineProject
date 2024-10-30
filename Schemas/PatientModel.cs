using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class PatientModel
    {
        [Required]public Guid id { get; set; }
        [Required] public string name { get; set; }
        [Required] public DateTime createTime { get; set; }
        [Required] public Gender gender { get; set; }
        public DateTime? birthday { get; set; }
    }
}
