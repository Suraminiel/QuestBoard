using System.ComponentModel.DataAnnotations;

namespace QuestBoard.Models.ViewModes
{
    public class AddTagRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
