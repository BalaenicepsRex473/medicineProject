using System.ComponentModel.DataAnnotations;

namespace scrubsAPI.Schemas
{
    public class TokenResponceModel
    {
        [Required] public string token { get; set; }
    }
}
