using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class ForumThreadsViewModel
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public ICollection<ForumPost> Postings { get; set; }

        public Guid ProjectId { get; set; }
        public Projects Project { get; set; }
    }
}
