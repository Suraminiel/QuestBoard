namespace QuestBoard.Models.Domain
{
    public class UserJobTask
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public Guid JobTaskId { get; set; }
        public JobTask JobTask { get; set; }
    }
}
