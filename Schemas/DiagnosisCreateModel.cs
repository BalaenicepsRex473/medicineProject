using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class DiagnosisCreateModel
    {
        [Required] public Guid icdDiagnosisId { get; set; }
        public string description { get; set; }
        [Required] public DiagnosisType type { get; set; }
    }
}
