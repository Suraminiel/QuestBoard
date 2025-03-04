namespace QuestBoard.Models.Domain
{
    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }
    public class JobTask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public ICollection<Guid> AdminUserRights { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<AppUser> Users { get; set; }  // aktuell gibt es eine many to many relationship zwischen AppUser und JobTask. wenn ich Project einfüge, muss das auf many to many von AppUser zu Projects geändert werden
       
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public PriorityLevel Priority { get; set; }
    }
}
