using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class DocumentViewModel
    {
        public string name { get; set; }
        public List<SingleDocumentViewModel> docs {  get; set; }

        public Guid ProjectId { get; set; } // Fremdschlüssel
        public Projects Project { get; set; } // Navigation Property
    }
}
