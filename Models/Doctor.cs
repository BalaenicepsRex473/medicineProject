using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class Doctor
    {

        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Gender gender { get; set; }
        public DateTime birthday { get; set; }
        public Guid speciality { get; set; }
        public DateTime createTime { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
    }
}
