namespace scrubsAPI.Models
{
    public class Diagnosis
    {
        public Guid id { get; set; }
        public virtual Icd10 icdDiagnosis { get; set; }
        public string description { get; set; }
        public DateTime createTime { get; set; }
        public DiagnosisType type { get; set; }
        public virtual Inspection inspection { get; set; }
    }
}
