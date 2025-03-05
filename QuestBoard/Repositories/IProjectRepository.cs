using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Projects>> GetAllAsync();
        Task<IEnumerable<Projects>?> GetAllForThisUserAsync(Guid id);

        Task<Projects?> GetAsync(Guid id);
        //   Task<JobTask?> GetByUrlHandleAsync(string urlHandle);
        Task<Projects> AddAsync(Projects projects);
        Task<Projects?> UpdateAsync(Projects jobTask);
        Task<Projects?> DeleteAsync(Guid id);
    }
}
