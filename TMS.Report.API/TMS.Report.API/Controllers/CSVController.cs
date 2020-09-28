using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TMS.Report.API.Models;

namespace TMS.Report.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CSVController : ControllerBase
    {
        private IConfiguration _configuration;
        public CSVController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/CSV")]
        [HttpGet]
        public async Task<IActionResult> Download(DateTime DateForSearch)
        {
            List<CSVModel> dataFromAPI = ConnectToTMSAPI();
            List<CSVModel> dataForCSV = new List<CSVModel>();
            var fileName = "report.csv";
            var path = @"d:\" + fileName;
            for (var i = 0; i < dataFromAPI.Count; i++)
            {
                if (dataFromAPI[i].State == "inProgress" && DateForSearch <= dataFromAPI[i].StartDate)
                {
                    dataForCSV.Add(dataFromAPI[i]);
                }
            }
            WriteCSV(dataForCSV, path);
            return PhysicalFile(path, "text/plain", fileName);
        }

        public void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/ConnectToTMSAPI")]
        [HttpGet]
        public List<CSVModel> ConnectToTMSAPI()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetConnectionString("RouteTMSAPI"));
            // Add an Accept header for JSON format.    
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all Names.    
            HttpResponseMessage response = client.GetAsync("Tasks").Result;  // Blocking call!    
            if (response.IsSuccessStatusCode)
            {
                var task = JsonConvert.DeserializeObject<List<CSVModel>>(response.Content.ReadAsStringAsync().Result);
                return task;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                var error = new List<CSVModel>();
                return error;
            }
        }
    }
}
