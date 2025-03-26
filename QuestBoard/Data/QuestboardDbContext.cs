using Microsoft.EntityFrameworkCore;
using QuestBoard.Models.Domain;

namespace QuestBoard.Data
{
    public class QuestboardDbContext : DbContext
    {
        public QuestboardDbContext(DbContextOptions<QuestboardDbContext> options) : base(options)
        {

        }

        


        public DbSet<JobTask> JobsAndTasks { get; set; }
        public DbSet<Subtask> Subtask { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AppUser> Users { get; set; }

        public DbSet<Projects> Projects { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<ForumThread> forumThreads { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
       
    }
}
