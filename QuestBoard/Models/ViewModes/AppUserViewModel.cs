using QuestBoard.Models.Domain;

namespace QuestBoard.Models.ViewModes
{
    public class AppUserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //   public ICollection<Guid> OwnedTasks { get; set; }
        //   public string UserID { get; set; }
        public ICollection<JobTask> JobTasks { get; set; } // Definiert wer leserechte an einer Aufgabe hat

        public ICollection<Projects> Projects { get; set; }

        public bool IsAdminOfSecelectedProject { get; set; }
    }
}
