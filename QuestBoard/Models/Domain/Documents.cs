namespace QuestBoard.Models.Domain
{
    public class Documents
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string path { get; set; }

        public Guid ProjectId { get; set; } // Fremdschlüssel
        public Projects Project { get; set; } // Navigation Property
    }
}
