using Blog_App.Models.Domain;

namespace Blog_App.Repository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteASync(Guid id);
    }
}
