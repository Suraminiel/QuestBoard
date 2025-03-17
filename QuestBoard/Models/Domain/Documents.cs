namespace QuestBoard.Models.Domain
{
    public class Documents
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public DateTime created { get; set; }

        public Guid UserId { get; set; } // Fremdschlüssel zu user der den file Hochgeladen hat
        public AppUser User { get; set; } // Navigation Property
        public Guid ProjectId { get; set; } // Fremdschlüssel
        public Projects Project { get; set; } // Navigation Property
    }
}
