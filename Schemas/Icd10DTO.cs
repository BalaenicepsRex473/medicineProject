namespace scrubsAPI
{
    public class Icd10DTO
    {
        public string code { get; set; }
        public Guid? parentId { get; set; }
        public string name { get; set; }
    }
}
