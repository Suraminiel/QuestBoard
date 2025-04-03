using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IQuestboardTaskRepository
    {
        Task<IEnumerable<JobTask>> GetAllAsync();
        Task<IEnumerable<JobTask>> GetAllForThisUserAsync(Guid id);
        Task<IEnumerable<JobTask>> GetAllForThisProjectAsync(Guid id, int pageNumber = 1, int pageSize = 100);
        Task<int> CountProjectTasksAsync(Guid id);
        Task<JobTask?> GetAsync(Guid id);
     //   Task<JobTask?> GetByUrlHandleAsync(string urlHandle);
        Task<JobTask> AddAsync(JobTask jobTask);
        Task<JobTask?> UpdateAsync(JobTask jobTask, string deletedSubtasks = null);
        Task<JobTask?> DeleteAsync(Guid id);
        Task<JobTask?> DeleteIndividualAsync(Guid id, Guid projectId);
    }
}

