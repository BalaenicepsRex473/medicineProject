namespace scrubsAPI
{
    public class DiagnosisCreateModel
    {
        public Guid icdDiagnosisId { get; set; }
        public string description { get; set; }
        public DiagnosisType type { get; set; }
    }
}
