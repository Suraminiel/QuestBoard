using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class ProjectsOverview
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string shortDescription { get; set; }

        public DateTime? creationTime { get; set; }

        public string creator { get; set; }

        
    }
}
