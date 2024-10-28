using scrubsAPI.Models;

namespace scrubsAPI
{
    public class Consultation
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public virtual Inspection inspection { get; set; }
        public virtual Speciality? speciality { get; set; }
    }
}