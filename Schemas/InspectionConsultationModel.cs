namespace scrubsAPI.Schemas
{
    public class InspectionConsultationModel
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public Guid? inspectionId { get; set; }
        public SpecialityModel? speciality { get; set; }
        public InspectionCommentModel? rootComment { get; set; }
        public int commentNumber { get; set; }

    }
}
