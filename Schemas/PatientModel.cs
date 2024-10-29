namespace scrubsAPI
{
    public class PatientModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime createTime { get; set; }
        public Gender gender { get; set; }
        public DateTime? birthday { get; set; }
    }
}
