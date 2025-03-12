using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class TaskOverview
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public float progress { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<AppUser> Users { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public PriorityLevel Priority { get; set; }
    }
}
