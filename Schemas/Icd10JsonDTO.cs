namespace scrubsAPI
{
    //Чисто для парсинга icd с сайта миндздрава 
    public class Icd10JsonDTO
    {
        public int ID { get; set; }
        public string REC_CODE { get; set; }
        public string MKB_CODE { get; set; }
        public string MKB_NAME { get; set; }
        public int? ID_PARENT { get; set; }
        public int ACTUAL { get; set; }
    }
}
