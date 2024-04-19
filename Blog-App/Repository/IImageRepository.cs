namespace Blog_App.Repository
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
