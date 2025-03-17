using Microsoft.EntityFrameworkCore;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public DocumentsRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }
        public async Task<Documents> AddAsync(Documents documents)
        {
          await questboardDbContext.AddAsync(documents);
          await questboardDbContext.SaveChangesAsync();
          return documents;
        }

        public async Task<Documents?> DeleteAsync(Guid id)
        {
            var Document = await questboardDbContext.Documents.FindAsync(id);
            if (Document != null)
            {
                 questboardDbContext.Documents.Remove(Document);
                 await questboardDbContext.SaveChangesAsync();
                return Document;
            }
            return null;
        }

        public async Task<IEnumerable<Documents>> GetAllOfThisProject(Guid ProjektId)
        {
           var test = await questboardDbContext.Documents.
                Where(d => d.ProjectId == ProjektId)
                .ToListAsync();
            return test;
        }

        public async Task<Documents?> GetAsync(Guid id)
        {
            return await questboardDbContext.Documents.FirstOrDefaultAsync( x => x.id == id);
        }
    }
}
