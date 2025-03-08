using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser?> GetAsync (Guid id);
        Task<AppUser?> GetAsync(string name);
        Task<AppUser> AddAsync(AppUser appUser);
        Task<AppUser?> UpdateAsync(AppUser appUser);
        Task<AppUser?> DeleteAsync(Guid id);
    }
}
