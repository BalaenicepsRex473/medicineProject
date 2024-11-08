using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class InspectionCommentCreateModel
    {
        [Required][MinLength(1), MaxLength(1000)]public string content { get; set; }
    }
}
