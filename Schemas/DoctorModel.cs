namespace scrubsAPI
{
    public class DoctorModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime birthsday { get; set; }
        public Gender gender { get; set; }
        public DateTime creteTime { get; set; }
        public string phone { get; set; }

        public SpecialityModel Speciality { get; set; }
    }
}
