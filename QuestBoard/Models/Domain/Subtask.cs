namespace QuestBoard.Models.Domain
{
    public class Subtask
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsCompleted { get; set; }
       
    }
}
