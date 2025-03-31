using System.ComponentModel.DataAnnotations;

namespace QuestBoard.Models.ViewModes
{
    public class AddProjectRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string shortDescription { get; set; }
    }
}
