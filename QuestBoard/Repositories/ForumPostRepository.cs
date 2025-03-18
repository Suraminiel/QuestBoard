using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class ForumPostRepository : IForumPostRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public ForumPostRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }
        public async Task<ForumPost> AddAsync(ForumPost post)
        {
            await questboardDbContext.AddAsync(post);
            await questboardDbContext.SaveChangesAsync();
            return post;
        }

        public Task<IEnumerable<ForumPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
