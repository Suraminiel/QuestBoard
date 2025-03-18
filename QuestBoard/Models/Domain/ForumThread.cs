namespace QuestBoard.Models.Domain
{
    public class ForumThread
    {
        public Guid id { get; set; }
        
        public string name { get; set; }

        public List<ForumPost> Postings { get; set; }

        public Guid ProjectId { get; set; }
        public Projects Project { get; set; }

       

    }
}
