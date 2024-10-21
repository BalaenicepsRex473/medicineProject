namespace scrubsAPI.Models
{
    public class Icd10
    {
        public string code { get; set; }
        public string name { get; set; }
        public Guid id { get; set; }
        public DateTime createTime { get; set; }

    }
}
