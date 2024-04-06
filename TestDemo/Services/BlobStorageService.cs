using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Reflection.Metadata;
using TestDemo.Configuration;
using TestDemo.Model;
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

        public async Task<InventoryItemExcel> ReadFile(string path)
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

            var item = new InventoryItemExcel();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    await blobClient.DownloadToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Load the Excel file into NPOI workbook
                    XSSFWorkbook workbook;
                    memoryStream.Position = 0;
                    workbook = new XSSFWorkbook(memoryStream);

                    // Get the first worksheet
                    ISheet worksheet = workbook.GetSheetAt(0); // Assuming there is only one worksheet

                    // Iterate through rows and columns
                    for (int row = 1; row <= worksheet.LastRowNum; row++)
                    {
                        IRow excelRow = worksheet.GetRow(row);
                        if (excelRow != null) // null is when the row only contains empty cells 
                        {
                           
                            item.SkuName = excelRow.GetCell(0).ToString();
                            item.Description = excelRow.GetCell(1).ToString();
                            item.Cost = Convert.ToInt32(excelRow.GetCell(2).ToString());
                            item.ProfitPerItem = Convert.ToInt32(excelRow.GetCell(3).ToString());
                            item.Quantity = Convert.ToInt32(excelRow.GetCell(4).ToString());
                            item.WasDeleted = Convert.ToInt32(excelRow.GetCell(5).ToString());
                        }
                    }
                    memoryStream.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return item;
        }
    }
}
