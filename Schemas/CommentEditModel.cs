using System.ComponentModel.DataAnnotations;

namespace scrubsAPI.Schemas
{
    public class CommentEditModel
    {
        [Required][MinLength(1), MaxLength(1000)]public string content { get; set; }
    }
}