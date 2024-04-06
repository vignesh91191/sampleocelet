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
                JObject json = JObject.Parse(mySbMsg);
                string excelFileUrl = (string)json["data"]["url"];
                var path = Path.GetFileName(excelFileUrl);

                string url = "https://localhost:7284/api/Currency/ReadData?path="+ path;

                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Create your request content if needed
                        HttpContent content = new StringContent("");

                        // Add headers if needed
                        content.Headers.Clear();
                        //content.Headers.Add("accept", "text/plain");

                        // Make the POST request
                        var response = await client.PostAsync(url, content);

                        // Check if the response is successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content
                            string responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Response from server:");
                            Console.WriteLine(responseContent);
                        }
                        else
                        {
                            // If the request was not successful, handle it accordingly
                            Console.WriteLine("Error: " + response.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
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
