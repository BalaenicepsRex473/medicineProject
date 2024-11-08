using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class SpecialityModel
    {
        [Required] public Guid id { get; set; }
        [Required] public DateTime createTime { get; set; }
        [Required] public string name { get; set; }
    }
}
