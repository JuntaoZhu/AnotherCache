using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace juzhucore.Controllers
{
    [Route("api/[controller]")]
    public class CachedTestController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var r = new Random();
            int i = r.Next(5000);

            string url = "/temp/" + i / 100 + "/test" + i + ".txt";
            return Redirect(url);
        }
    }
}
