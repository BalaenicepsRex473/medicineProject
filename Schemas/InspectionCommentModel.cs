using System.ComponentModel.DataAnnotations;

namespace scrubsAPI.Schemas
{
    public class InspectionCommentModel
    {
        [Required]public Guid id { get; set; }
        [Required]public DateTime createTime { get; set; }
        public Guid? parentId { get; set; }
        [Required][MinLength(1), MaxLength(1000)] public string content { get; set; }
        public DoctorModel author { get; set; }
        public DateTime? modifyTime { get; set; }
    }
}
