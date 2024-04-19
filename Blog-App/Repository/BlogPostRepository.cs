using Blog_App.Data;
using Blog_App.Models.Domain;
using Blog_App.Models.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Blog_App.Repository
{
    public class BlogPostRepository:IBlogPostRepository
    {
        private readonly BlogDbContext dbContext;

        public BlogPostRepository(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await dbContext.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteASync(Guid id)
        {
            var existingBlog = await dbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                dbContext.BlogPosts.Remove(existingBlog);
                await dbContext.SaveChangesAsync();
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
           return await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsynct(string urlHandle)
        {
          return await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingblog = await dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingblog != null)
            {
                existingblog.Id = blogPost.Id;
                existingblog.Heading = blogPost.Heading;
                existingblog.PageTitle = blogPost.PageTitle;
                existingblog.Content = blogPost.Content;
                existingblog.ShortDescription = blogPost.ShortDescription;
                existingblog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingblog.UrlHandle = blogPost.UrlHandle;
                existingblog.PublishedDate = blogPost.PublishedDate;
                existingblog.Author = blogPost.Author;
                existingblog.Visible = blogPost.Visible;
                existingblog.Tags = blogPost.Tags;

                await dbContext.SaveChangesAsync();
                return existingblog;
            }
            return null;
        }
    }
}
