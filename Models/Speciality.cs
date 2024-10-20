using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class Speciality
    {
        [Key]
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public string name { get; set; }
    }
}
