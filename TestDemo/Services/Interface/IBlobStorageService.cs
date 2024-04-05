namespace TestDemo.Services.Interface
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
