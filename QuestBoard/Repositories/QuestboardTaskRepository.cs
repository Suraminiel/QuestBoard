using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class QuestboardTaskRepository : IQuestboardTaskRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public QuestboardTaskRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }
        public async Task<JobTask> AddAsync(JobTask jobTask)
        {
           await questboardDbContext.AddAsync(jobTask);
           await questboardDbContext.SaveChangesAsync(); 
           return jobTask;
        }

        public async Task<JobTask?> DeleteAsync(Guid id)
        {
            var existingJobTask = await questboardDbContext.JobsAndTasks.FindAsync(id);

            if (existingJobTask != null)
            {
                questboardDbContext.JobsAndTasks.Remove(existingJobTask);
                await questboardDbContext.SaveChangesAsync();
                return existingJobTask;
            }
            return null;
        }

        public async Task<IEnumerable<JobTask>> GetAllAsync()
        {
            return await questboardDbContext.JobsAndTasks.Include(x => x.Tags).ToListAsync();
        }

        public async Task<JobTask?> GetAsync(Guid id)
        {
            return await questboardDbContext.JobsAndTasks.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<JobTask?> UpdateAsync(JobTask jobTask)
        {
            var existingJobTask = await questboardDbContext.JobsAndTasks.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == jobTask.Id);
            if (existingJobTask != null)
            {
                existingJobTask.Id = jobTask.Id;
                existingJobTask.Name = jobTask.Name;
                existingJobTask.Description = jobTask.Description;
                existingJobTask.Type = jobTask.Type;
                existingJobTask.PublishedDate = jobTask.PublishedDate;
                existingJobTask.Author = jobTask.Author;
                existingJobTask.Tags = jobTask.Tags;

                await questboardDbContext.SaveChangesAsync();
                return existingJobTask;
            }
            return null;
        }
    }
}
