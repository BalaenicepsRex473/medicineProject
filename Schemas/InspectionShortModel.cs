namespace scrubsAPI
{
    public class InspectionShortModel
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public DateTime date { get; set; }
        public List <DiagnosisModel> diagnoses{ get; set; }
    }
}
