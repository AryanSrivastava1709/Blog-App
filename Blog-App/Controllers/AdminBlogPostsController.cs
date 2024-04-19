using Blog_App.Models.Domain;
using Blog_App.Models.ViewModels;
using Blog_App.Repository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog_App.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]

        public async Task<IActionResult> Add()
        {
            //get tag repository
            var tags = await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest) 
        {
            //Map viewModel to DomainModel
            var model = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle= addBlogPostRequest.PageTitle,
                Content= addBlogPostRequest.Content,
                ShortDescription= addBlogPostRequest.ShortDescription,
                FeaturedImageUrl= addBlogPostRequest.FeaturedImageUrl,
                UrlHandle= addBlogPostRequest.UrlHandle,
                PublishedDate= addBlogPostRequest.PublishedDate,
                Author= addBlogPostRequest.Author,
                Visible= addBlogPostRequest.Visible,
            };

            //map tags from selected id of tags
            var selectTags = new List<Tag>();
            foreach(var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectTags.Add(existingTag);
                }
            }

            //mapping tags back to the domain model
            model.Tags = selectTags;

            await blogPostRepository.AddAsync(model);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            var tags = await tagRepository.GetAllAsync();
            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tags.Select(x => new SelectListItem { Text=x.Name,Value=x.Id.ToString()}),
                    SelectedTags=blogPost.Tags.Select(x=>x.Id.ToString()).ToArray(),
                };
                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //Map viewModel to DomainModel
            var model = new BlogPost
            {
                Id= editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
            };

            //map tags from selected id of tags
            var selectTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null)
                {
                    selectTags.Add(existingTag);
                }
            }

            //mapping tags back to the domain model
            model.Tags = selectTags;

            var updatedBlog = await blogPostRepository.UpdateAsync(model);
            if (updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
           var deletedBlog = await blogPostRepository.DeleteASync(editBlogPostRequest.Id);
            if (deletedBlog != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id=editBlogPostRequest.Id});
        }
    }
}
