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

        public async Task<Projects?> DeleteAsync(Guid id)
        {
            var existingProject = await questboardDbContext.Projects.FindAsync(id);

            if (existingProject != null)
            {
                questboardDbContext.Projects.Remove(existingProject);
                await questboardDbContext.SaveChangesAsync();
                return existingProject;
            }
            return null;
        }

        public async Task<IEnumerable<Projects>> GetAllAsync()
        {
            return await questboardDbContext.Projects.ToListAsync();
        }

        public async Task<IEnumerable<Projects>?> GetAllForThisUserAsync(Guid id)
        {
            return await questboardDbContext.Projects.Where(jt => jt.Users.Any(u => u.Id == id)).ToListAsync();
        }

        public async Task<Projects?> GetAsync(Guid id)
        {
            return await questboardDbContext.Projects
                .Include(p => p.JobTasks)
                 .ThenInclude(jt => jt.Subtasks)
                .Include(p => p.JobTasks)
                 .ThenInclude(jt => jt.Tags)
                .FirstOrDefaultAsync(jt => jt.Id == id);
            //return await questboardDbContext.JobsAndTasks.Include(x => x.Tags).Include(st => st.Subtasks).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Projects?> UpdateAsync(Projects jobTask)
        {
            throw new NotImplementedException();
        }
    }
}
