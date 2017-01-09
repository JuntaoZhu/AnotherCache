using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace juzhucore.Controllers
{
    [Route("api/[controller]")]
    public class LocalFileController : Controller
    {
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return GetContent();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        string GetPath(int i)
        {
            string root = Path.GetFullPath(@"D:\local\LocalAppData");
            string path = root + @"\temp\" + i / 100 + @"\";
            return path;
        }

        public string GetContent()
        {
            var r = new Random();
            int i = r.Next(5000);

            string path = GetPath(i);
            string filename = path + "test" + i + ".txt";
            if (!System.IO.File.Exists(filename))
            {
                return "Not found";
            }

            FileInfo info = new FileInfo(filename);
            DateTime mofify = info.CreationTime;

            using (var stream = System.IO.File.OpenRead(filename))
            {
                var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}
