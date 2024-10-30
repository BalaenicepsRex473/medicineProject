using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class ConsultationCreateModel
    {
        [Required] public Guid specialityId { get; set; }
        [Required] public InspectionCommentCreateModel comment { get; set; }
    }
}
