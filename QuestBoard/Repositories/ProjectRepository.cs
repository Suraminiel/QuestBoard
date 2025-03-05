using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public ProjectRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }
        public async Task<Projects> AddAsync(Projects projects)
        {
            await questboardDbContext.AddAsync(projects);
            await questboardDbContext.SaveChangesAsync();
            return projects;
            
        }

        public Task<Projects?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Projects>> GetAllAsync()
        {
            return await questboardDbContext.Projects.ToListAsync();
        }

        public async Task<IEnumerable<Projects>?> GetAllForThisUserAsync(Guid id)
        {
            return await questboardDbContext.Projects.Where(jt => jt.Users.Any(u => u.Id == id)).ToListAsync();
        }

        public Task<Projects?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Projects?> UpdateAsync(Projects jobTask)
        {
            throw new NotImplementedException();
        }
    }
}
