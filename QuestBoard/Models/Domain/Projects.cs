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
        public ICollection<AppUser> Users { get; set; }  // aktuell gibt es eine many to many relationship zwischen AppUser und
                                                         // JobTask. wenn ich Project einfüge, muss das auf many to many von AppUser
                                                         // zu Projects geändert werden

        public ICollection<JobTask> JobTasks { get; set; }

        public ICollection<Documents> Documents { get; set; }

        public ICollection<ForumThread> Threads { get; set; }

   
    }
}
