using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public interface IDocumentsRepository
    {
        Task<IEnumerable<Documents>> GetAllOfThisProject(Guid ProjektId);
        Task<Documents?> GetAsync(Guid id);
        Task<Documents> AddAsync(Documents documents);

    }
}
