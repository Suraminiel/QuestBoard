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
            return await questboardDbContext.JobsAndTasks.Include(x => x.Tags).Include(st => st.Subtasks).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<JobTask?> UpdateAsync(JobTask jobTask, string deletedSubtasks)
        {
            var existingJobTask = await questboardDbContext.JobsAndTasks.Include(x => x.Tags).Include(st => st.Subtasks).FirstOrDefaultAsync(x => x.Id == jobTask.Id);

            // delete subtasks that where deleted by the user
            if (!string.IsNullOrEmpty(deletedSubtasks))
            {
                var subtasksToDelte = deletedSubtasks.Split(',')
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(Guid.Parse)
                    .ToList();

                var subtaskToRemove = questboardDbContext.Subtask.Where(st => subtasksToDelte.Contains(st.Id));
                questboardDbContext.Subtask.RemoveRange(subtaskToRemove);
            }

            //update Task
            if (existingJobTask != null)
            {
                existingJobTask.Id = jobTask.Id;
                existingJobTask.Name = jobTask.Name;
                existingJobTask.Description = jobTask.Description;
                //existingJobTask.Subtasks = jobTask.Subtasks;
                existingJobTask.Type = jobTask.Type;
                existingJobTask.PublishedDate = jobTask.PublishedDate;
                existingJobTask.Author = jobTask.Author;
                existingJobTask.Priority = jobTask.Priority;
                existingJobTask.Tags = jobTask.Tags;

                foreach (var subtask in jobTask.Subtasks)
                {
                    var exisitingSubtask = questboardDbContext.Subtask.FirstOrDefault(s => s.Id == subtask.Id);

                    if (exisitingSubtask == null)
                    {
                        existingJobTask.Subtasks.Add(subtask);
                    }
                    else
                    {
                        exisitingSubtask.Name = subtask.Name;
                        exisitingSubtask.IsCompleted = subtask.IsCompleted;
                    }
                }

                await questboardDbContext.SaveChangesAsync();
                return existingJobTask;
            }
            return null;
        }
    }
}
