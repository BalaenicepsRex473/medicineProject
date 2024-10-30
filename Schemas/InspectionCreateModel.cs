using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class InspectionCreateModel
    {
        [Required] public DateTime date { get; set; }
        [Required][MinLength(1), MaxLength(5000)] public string anamesis { get; set; }
        [Required][MinLength(1), MaxLength(5000)] public string complaints { get; set; }
        [Required][MinLength(1), MaxLength(5000)] public string treatment { get; set; }
        [Required] public Conclusion conclusion { get; set; }
        public DateTime? nextVisitDate { get; set; }
        public DateTime? deathTime { get; set; }
        public Guid? previousInspectionId { get; set; }
        [Required][MinLength(1)] public List<DiagnosisCreateModel> diagnoses { get; set; }
        public List<ConsultationCreateModel> consultations { get; set; }
    }
}