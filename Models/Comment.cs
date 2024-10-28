namespace scrubsAPI.Models
{
    public class Comment
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? modifiedTime { get; set; }
        public string content { get; set; }
        public virtual Doctor author { get; set; }
        public virtual Comment? parentComment { get; set;}
        public virtual Consultation consultation { get; set;}
    }
}
