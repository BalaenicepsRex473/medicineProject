using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class InspectionEditModel
    {
        public string? anamesis { get; set; }
        [Required] public string complaints { get; set; }
        [Required] public string treatment { get; set; }
        [Required] public Conclusion conclusion { get; set; }
        public DateTime? nextVisitDate { get; set; }
        public DateTime? deathTime { get; set; }
        [Required] public List<DiagnosisCreateModel> diagnoses { get; set; }
    }
}
