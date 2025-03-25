using QuestBoard.Models.Domain;
using System.Globalization;

namespace QuestBoard.Models.ViewModes
{
    public class SingleDocumentViewModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public DateTime created { get; set; }
        public string uploaderName { get; set; }
        public string uploaderProfilePicPath { get; set; }
        public bool isAuthorizedToEdit { get; set; }
        public Guid UserId { get; set; } // Fremdschlüssel zu user der den file Hochgeladen hat
        public Guid ProjectId { get; set; } // Fremdschlüssel
        public Projects Project { get; set; } // Navigation Property
    }
}
