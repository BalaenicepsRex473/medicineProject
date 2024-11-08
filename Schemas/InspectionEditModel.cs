using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class InspectionEditModel
    {
        [Required][MinLength(1), MaxLength(1000)] public string anamesis { get; set; }
        [Required][MinLength(1), MaxLength(1000)] public string complaints { get; set; }
        [Required][MinLength(1), MaxLength(1000)] public string treatment { get; set; }
        [Required] public Conclusion conclusion { get; set; }
        public DateTime? nextVisitDate { get; set; }
        public DateTime? deathTime { get; set; }
        [Required][MinLength(1)] public List<DiagnosisCreateModel> diagnoses { get; set; }
    }
}
