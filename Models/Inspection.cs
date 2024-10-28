namespace scrubsAPI
{
    public class Inspection
    {
        public virtual Patient patient { get; set; }
        public Guid id { get; set; }
        public DateTime date { get; set; }
        public DateTime createTime { get; set; }
        public string anamesis { get; set; }
        public string treatment { get; set; }
        public string complaints { get; set; }
        public Conclusion conclusion { get; set; }
        public DateTime? nextVisitDate { get; set; }
        public DateTime? deathTime { get; set;}
        public virtual Inspection? previousInspection { get; set; }
        public virtual Doctor doctor { get; set; }
    }
}
