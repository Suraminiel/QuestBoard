namespace QuestBoard.Models.Domain
{
    public class ForumPost
    {
        public Guid id { get; set; }
        public string name { get; set; }

        public DateTime created { get; set; }
        public string message { get; set; }

        public Guid ThreadId { get; set; }
        public ForumThread Thread { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
