using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IForumThreadRepository
    {
        Task<IEnumerable<ForumThread>> GetAllAsync();
    }
}
