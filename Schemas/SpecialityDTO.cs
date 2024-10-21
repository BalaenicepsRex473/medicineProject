using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class SpecialityDTO
    {
        [Required] public string name { get; set; }
    }
}
