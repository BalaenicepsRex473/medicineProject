using scrubsAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class CommentModel
    {
        [Required]public Guid id { get; set; }
        [Required] public DateTime createTime { get; set; }
        public DateTime? modifiedTime { get; set; }
        [Required][MinLength(1), MaxLength(1000)] public string content { get; set; }
        [Required][MinLength(1)] public string author { get; set; }
        [Required] public Guid authorId { get; set; }
        public Guid? parentId { get; set; }
    }
}
