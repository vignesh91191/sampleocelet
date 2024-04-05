using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using TestDemo.Configuration;
using TestDemo.Services.Interface;

namespace TestDemo.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        
        public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings)
        {
            var settings = blobStorageSettings.Value;
            _blobServiceClient = new BlobServiceClient(settings.ConnectionString);
            _containerName = settings.ContainerName;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("File is not provided");

                var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName)
                    .GetBlobClient(Path.GetFileName(file.FileName));

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file to blob storage: {ex.Message}");
            }
        }
    }
}
