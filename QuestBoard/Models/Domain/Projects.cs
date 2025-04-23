namespace QuestBoard.Models.Domain
{
    public class Projects
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? shortDescription { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<Guid> AdminUserRights { get; set; }
        public ICollection<AppUser> Users { get; set; }  

        public ICollection<JobTask> JobTasks { get; set; }

        public ICollection<Documents> Documents { get; set; }

        public ICollection<ForumThread> Threads { get; set; }

   
    }
}
