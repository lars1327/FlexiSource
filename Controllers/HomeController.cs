using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using opg_201910_interview.Models;

namespace opg_201910_interview.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration configuration;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration iConfig)
        {
            _logger = logger;
            configuration = iConfig;
        }

        public IActionResult ClientA()
        {
            List<ClientFile> files = ProcessDirectory(configuration.GetSection("ClientSettings").GetSection("ClientA").GetSection("FileDirectoryPath").Value);

            return Ok(files.OrderBy(o => o.FileDate).Select(p => p.Name).Distinct());
        }

        public IActionResult ClientB()
        {
            List<ClientFile> files = ProcessDirectory(configuration.GetSection("ClientSettings").GetSection("ClientB").GetSection("FileDirectoryPath").Value);

            return Ok(files.OrderBy(o => o.FileDate).Select(p => p.Name).Distinct());
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<ClientFile> ProcessDirectory(string client)
        {
            List<ClientFile> result = new List<ClientFile>();
            
            string[] fileEntries = Directory.GetFiles(client);
            int x,y;
            DateTime timeformat;
            foreach (string fileName in fileEntries)
            {
                try
                {
                    var fi2 = new FileInfo(fileName);
                    var fName = fi2.Name;
                    x = fName.IndexOf("-", 1, fName.Count() - 1) < 0 ? fName.IndexOf("_", 1, fName.Count() - 1) : fName.IndexOf("-", 1, fName.Count() - 1);
                    y = fName.IndexOf(".", 1, fName.Count() - 1);
                    var name = fName.Substring(0, x);
                    fName = fName.Substring(x + 1, y - (x + 1)); 
                    fName = fName.Replace("-", "");
                
                    timeformat = DateTime.ParseExact(fName, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None);
                    result.Add(new ClientFile { Filename = fi2.Name, Name= name, FileDate = timeformat, FileExtension = "ext" });
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }

            return result;
        }

    }
}
