using Blog_App.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog_App.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandleAsynct(urlHandle);
            return View(blogPost);
        }
    }
}
