using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using SynchData.Model;

namespace SynchData
{
    public class SynchCurrency
    {
        private readonly ILogger<SynchCurrency> _logger;

        public SynchCurrency(ILogger<SynchCurrency> log)
        {
            _logger = log;
        }

        [FunctionName("SynchCurrency")]
        public async Task RunAsync([ServiceBusTrigger("currency", "currencySave", Connection = "ServiceBusConnection")] string mySbMsg)
        {
            try
            {
               

                //// Assuming the message contains the Excel file bytes, deserialize it and read the data
                //byte[] excelFileBytes = Convert.FromBase64String(mySbMsg);

                //// Create a stream from the byte array
                //using (MemoryStream stream = new MemoryStream(excelFileBytes))
                //{
                //    // Load the Excel package
                //    using (ExcelPackage package = new ExcelPackage(stream))
                //    {
                //        // Assuming the Excel file has only one worksheet
                //        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                //        // Iterate through each row to read data
                //        for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                //        {
                //            var item = new InventoryItem();
                //            item.SkuName = worksheet.Cells[row, 1].Value?.ToString();
                //            item.Description = worksheet.Cells[row, 2].Value?.ToString();
                //            item.Cost = Convert.ToInt64(worksheet.Cells[row, 3].Value);
                //            item.ProfitPerItem = Convert.ToInt64(worksheet.Cells[row, 4].Value);
                //            item.Quantity = Convert.ToInt32(worksheet.Cells[row, 5].Value);
                //            item.WasDeleted = Convert.ToInt32(worksheet.Cells[row, 6].Value);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during processing
                _logger.LogError($"An error occurred: {ex.Message}");
                // Optionally, rethrow the exception if needed
                // throw;
            }
        }
    }
}
