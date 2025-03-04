using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class AppUserRepository: IAppUserRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public AppUserRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }

        public async Task<AppUser> AddAsync(AppUser appUser)
        {
            await questboardDbContext.AddAsync(appUser);
            await questboardDbContext.SaveChangesAsync();
            return appUser;
        }

        public Task<AppUser?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser?> GetAsync(Guid id)
        {
            return await questboardDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            //return await questboardDbContext.JobsAndTasks.Include(x => x.Tags).Include(st => st.Subtasks).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<AppUser?> UpdateAsync(AppUser appUser)
        {
            throw new NotImplementedException();
        }
    }
}
