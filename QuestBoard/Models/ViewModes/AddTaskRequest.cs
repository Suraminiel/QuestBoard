using Microsoft.AspNetCore.Mvc.Rendering;
using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class AddTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }

        public PriorityLevel Priority { get; set; }

        //Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }
        // Collect Tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();

    }
}
