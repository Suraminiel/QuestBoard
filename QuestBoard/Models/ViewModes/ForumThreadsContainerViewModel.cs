using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class ForumThreadsContainerViewModel
    {
        public Guid ProjectId { get; set; }
        public Projects Project { get; set; } // Navigation Property
    
        public ICollection<ForumThreadsViewModel> ForumThreads { get; set; }
        public ForumPost newMessage { get; set; } = new ForumPost();
    }
}
