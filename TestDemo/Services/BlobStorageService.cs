using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using OfficeOpenXml;
using System.Reflection.Metadata;
using TestDemo.Configuration;
using TestDemo.Services.Interface;

namespace TestDemo.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _blobName;
        private readonly string _connectionString;
        public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings)
        {
            var settings = blobStorageSettings.Value;
            _blobServiceClient = new BlobServiceClient(settings.ConnectionString);
            _containerName = settings.ContainerName;
            _blobName = settings.BlobName;
            _connectionString = settings.ConnectionString;
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

        public async Task<string> ReadFile(string path)
        {

            string fileName = Path.GetFileName(path);
            string connectionString = _connectionString;
            string containerName = _containerName;
            string blobName = _blobName;

            // Create a BlobServiceClient object using the connection string
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Download the blob's content to a memory stream
            MemoryStream memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Read Excel data from the memory stream using EPPlus
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0]; // Assuming there is only one worksheet
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        Console.Write(worksheet.Cells[row, col].Value?.ToString() + "\t");
                    }
                    Console.WriteLine();
                }
            }

            // Dispose the memory stream
            memoryStream.Dispose();

            return "";
        }
    }
}
