using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class Patient
    {

        [Key] 
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime? birthDay { get; set; }

        public DateTime creationTime { get; set; }

        public Sex gender { get; set; }
    }
}
