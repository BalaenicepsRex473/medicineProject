using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class SpecialityCreateModel
    {
        [Required][MinLength(1)] public string name { get; set; }
    }
}
