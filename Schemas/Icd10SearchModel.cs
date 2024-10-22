namespace scrubsAPI.Schemas
{
    public class Icd10SearchModel
    {
        public List<Icd10RecordModel> Icd10s { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
