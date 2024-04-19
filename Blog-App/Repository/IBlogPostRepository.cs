using Blog_App.Models.Domain;

namespace Blog_App.Repository
{
    public interface IBlogPostRepository
    {

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsynct(string urlHandle);

        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteASync(Guid id);
    }
}
