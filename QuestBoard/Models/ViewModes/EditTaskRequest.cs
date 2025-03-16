using Microsoft.AspNetCore.Mvc.Rendering;
using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class EditTaskRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public string DeletedSubtaskIds { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public PriorityLevel Priority { get; set; }
        //Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }

        // Collect Tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();


        public IEnumerable<SelectListItem> Users { get; set; }
        public string[] SelectedUsers { get; set; } = Array.Empty<string>();

        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
