namespace scrubsAPI
{
    public class DoctorRegisterModel
    {
        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public DateTime birthsday { get; set; }
        public Gender gender { get; set; }

        public string phone { get; set; }

        public Guid speciality { get; set; }
    }
}
