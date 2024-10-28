namespace scrubsAPI
{
    public class InspectionCreateModel
    {
        public DateTime date { get; set; }
        public string anamesis { get; set; }
        public string complaints { get; set; }
        public string treatment { get; set; }
        public Conclusion conclusion { get; set; }
        public DateTime? nextVisitDate { get; set; }
        public DateTime? deathTime { get; set; }
        public Guid? previousInspectionId { get; set; }
        public List<DiagnosisCreateModel> diagnoses { get; set; }
        public List<ConsultationCreateModel> consultations { get; set; }
    }
}