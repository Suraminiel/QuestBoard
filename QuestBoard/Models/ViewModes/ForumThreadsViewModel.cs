using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class ForumThreadsViewModel
    {
        public Guid id { get; set; }
        public DateTime created { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string authorProfilePicturePath { get; set; }
        public bool isAuthorizedToEdit { get; set; } = false;
        public List<ForumPost> Postings { get; set; }

        public Guid ProjectId { get; set; }
        public Projects Project { get; set; }
    }
}
