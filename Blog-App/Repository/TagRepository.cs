using Blog_App.Data;
using Blog_App.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Blog_App.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext DbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            this.DbContext = blogDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await DbContext.Tags.AddAsync(tag);
            await DbContext.SaveChangesAsync();
            return tag;

        }

        public async Task<Tag?> DeleteASync(Guid id)
        {
            var existingTag = await DbContext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                DbContext.Tags.Remove(existingTag);
                await DbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await DbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return DbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await DbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name=tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await DbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
