namespace scrubsAPI.Models
{
    public class Icd10
    {
        public string code { get; set; }
        public string name { get; set; }
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public Guid? parentId { get; set; } = null;

        public int? parentIdFromJson { get; set; }
        public int? idFromJson { get; set; }


        public Icd10? parent { get; set; } = null;

    }
}
