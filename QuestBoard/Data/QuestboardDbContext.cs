using Microsoft.EntityFrameworkCore;
using QuestBoard.Models.Domain;

namespace QuestBoard.Data
{
    public class QuestboardDbContext : DbContext
    {
        public QuestboardDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<JobTask> JobsAndTasks { get; set; }
        public DbSet<Subtask> Subtask { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
