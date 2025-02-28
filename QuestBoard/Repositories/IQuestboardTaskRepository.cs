using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IQuestboardTaskRepository
    {
        Task<IEnumerable<JobTask>> GetAllAsync();
        Task<JobTask?> GetAsync(Guid id);
     //   Task<JobTask?> GetByUrlHandleAsync(string urlHandle);
        Task<JobTask> AddAsync(JobTask jobTask);
        Task<JobTask?> UpdateAsync(JobTask jobTask);
        Task<JobTask?> DeleteAsync(Guid id);
    }
}

