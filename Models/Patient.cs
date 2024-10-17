namespace scrubsAPI
{
    public class Patient
    {
        private Guid Id { get; }
        public string Name { get; }
        public DateTime BirthDay { get; }

        private DateTime CreationTime { get; }

        public Sex Sex { get; }
    }
}
