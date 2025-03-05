using Microsoft.AspNetCore.Identity;

namespace QuestBoard.Models.Domain
{
    public class AppUser
    {
        public Guid Id { get; set; }

     //   public ICollection<Guid> OwnedTasks { get; set; }
     //   public string UserID { get; set; }
        public ICollection<JobTask> JobTasks { get; set; } // Definiert wer leserechte an einer Aufgabe hat

        public ICollection<Projects> Projects  { get; set; }


    }
}
