using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class ForumPostViewModel
    {

        public Guid id { get; set; }

        public Guid ProjectId { get; set; }
        public string name { get; set; }

        public string ThreadName { get; set; }
        public string message { get; set; }
        public string newMessage { get; set; }


        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        
        public Guid ThreadId { get; set; }
        public ForumThread Thread { get; set; }

       
        
    }
}
