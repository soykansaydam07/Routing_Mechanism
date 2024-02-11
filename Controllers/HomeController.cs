using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Routing_Mechanism.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Routing_Mechanism.Controllers
{
 //   [Route("[controller]/[action]")] // Bir şablon tanımlaması yapılıcak attribute tanımlaması olduğu için köşeli parantez kullanılırken default(ön tanımlılar) olmayıp custom verilerde eklenicekse o zaman süslü parantez ile eklenme sağlanır
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string Id, string x, string y)
        {
            return View();
        }

   //     [Route("[act]/{id?}")] //Bu şekilde istenirse action bazlı iç veride verilmektedir  id  kısmına  tanımlı olmadığı için süslü paranteze alınmadı 
        public IActionResult Privacy(int? id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
