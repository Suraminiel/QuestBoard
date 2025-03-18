using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IForumPostRepository
    {
        Task<IEnumerable<ForumPost>> GetAllAsync();
        Task<ForumPost> AddAsync(ForumPost post);
    }
}
