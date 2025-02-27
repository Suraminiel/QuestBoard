using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using QuestBoard.Data;
using QuestBoard.Models.Domain;

namespace QuestBoard.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly QuestboardDbContext questboardDbContext;

        public TagRepository(QuestboardDbContext questboardDbContext)
        {
            this.questboardDbContext = questboardDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await questboardDbContext.Tags.AddAsync(tag);
            await questboardDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
           var existingTag = await questboardDbContext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                questboardDbContext.Tags.Remove(existingTag);
                await questboardDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            //var query = questboardDbContext.Tags.AsQueryable();
                
            return await questboardDbContext.Tags.ToListAsync();
              
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return questboardDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await questboardDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await questboardDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
