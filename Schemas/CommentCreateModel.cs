using System.ComponentModel.DataAnnotations;

namespace scrubsAPI.Schemas
{
    public class CommentCreateModel
    {
        [Required]public string content { get; set; }
        public Guid? parentId { get; set; }
    }
}
