using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class ForumThreadRepository : IForumThreadRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public ForumThreadRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }
        public async Task<ForumThread> AddAsync(ForumThread forumThread)
        {
            await questboardDbContext.AddAsync(forumThread);
            await questboardDbContext.SaveChangesAsync();
            return forumThread;
        }


        public async Task<IEnumerable<ForumThread>?> GetAllAsyncForThisProject(Guid ProjectId)
        {
            return await questboardDbContext.forumThreads.Include(p => p.Postings).Where(x => x.ProjectId == ProjectId).ToListAsync();
        }

        public async Task<ForumThread?> GetAsync(Guid id)
        {
            return await questboardDbContext.forumThreads.Include(p => p.Postings).FirstOrDefaultAsync(x => x.id == id);
        }
    }
}
