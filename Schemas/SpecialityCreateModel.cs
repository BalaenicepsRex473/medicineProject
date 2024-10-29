using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class SpecialityCreateModel
    {
        [Required] public string name { get; set; }
    }
}
