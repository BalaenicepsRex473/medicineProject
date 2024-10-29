using scrubsAPI.Models;

namespace scrubsAPI
{
    public class CommentModel
    {
        public Guid id { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? modifiedTime { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public Guid authorId { get; set; }
        public Guid? parentId { get; set; }
    }
}
