using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class Icd10RecordModel
    {
        [Required]public string code { get; set; }
        public string name { get; set; }
        [Required]public DateTime createTime { get; set; }

        public Guid id { get; set; }


    }
}
