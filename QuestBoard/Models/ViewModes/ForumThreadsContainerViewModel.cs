namespace QuestBoard.Models.ViewModes
{
    public class ForumThreadsContainerViewModel
    {
        public Guid ProjectId { get; set; }

        public ICollection<ForumThreadsViewModel> ForumThreads { get; set; }
    }
}
