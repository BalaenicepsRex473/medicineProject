﻿namespace scrubsAPI.Schemas
{
    public class ConsultationModel
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public Guid inspectionId { get; set; }
        public SpecialityModel speciality { get; set; }
        public List<CommentModel> comments { get; set; }
    }
}
