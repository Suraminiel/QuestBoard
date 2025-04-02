using Microsoft.AspNetCore.Identity;

namespace QuestBoard.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
