using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class ForumThreadRepository : IForumThreadRepository
    {
        public Task<IEnumerable<ForumThread>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
