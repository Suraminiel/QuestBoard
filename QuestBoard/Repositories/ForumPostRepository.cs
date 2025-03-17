using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class ForumPostRepository : IForumPostRepository
    {
        public Task<IEnumerable<ForumPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
