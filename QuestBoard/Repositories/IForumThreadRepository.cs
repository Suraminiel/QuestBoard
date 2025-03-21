using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IForumThreadRepository
    {
        Task<IEnumerable<ForumThread>?> GetAllAsyncForThisProject(Guid ProjectId);
        Task<ForumThread> AddAsync(ForumThread forumThread);
        Task<ForumThread?> GetAsync(Guid id);
        Task<ForumThread?> DeleteAsync(Guid id);
       
    }
}
