namespace scrubsAPI
{
    public class DiagnosisModel
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DiagnosisType type { get; set; }
    }
}
