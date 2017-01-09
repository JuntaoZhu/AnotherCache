using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using juzhucore.Models;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace juzhucore.Controllers
{
    public class LocalStaticTestController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new StaticTestForm { FilesCount = 5000, FileSize = 5 };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(StaticTestForm model)
        {
            switch (model.Submit)
            {
                case "Initialize":
                    doInitialize(model.FilesCount, model.FileSize);
                    ViewBag.Message = "Test files created";
                    break;
                case "CleanUp":
                    doCleanUp();
                    ViewBag.Message = "Cleaned up";
                    break;
                case "LoadTest":
                    ViewBag.StartTime = DateTime.Now;
                    doLoadTest(model.FilesCount);
                    ViewBag.StopTime = DateTime.Now;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return View("Index");
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

        string GetPath(int i)
        {
            string root = Path.GetFullPath(@"D:\local\LocalAppData");
            string path = root + @"\temp\" + i / 100 + @"\";
            return path;
        }

        public string PathOf(int i)
        {
            return GetPath(i);
        }

        void doInitialize(int filesCount, int fileSize)
        {
            char[] chars = new char[fileSize];
            for (int i = 0; i < fileSize; i++)
            {
                chars[i] = 'a';
            }

            for (int i = 0; i < filesCount; i++)
            {
                string path = GetPath(i);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string filename = path + "test" + i + ".txt";
                var f = System.IO.File.Open(filename, FileMode.OpenOrCreate);
                var w = new StreamWriter(f, System.Text.Encoding.UTF8);
                w.Write(chars);
                w.Dispose();
                f.Dispose();
            }
        }

        void doCleanUp()
        {
            string root = Path.GetFullPath(".");
            Directory.Delete(root + @"\temp", true);
        }

        void doLoadTest(int filesCount)
        {
            for (int i = 0; i < filesCount; i++)
            {
                string path = GetPath(i);
                string filename = path + "test" + i + ".txt";
                if (!System.IO.File.Exists(filename))
                {
                    break;
                }

                FileInfo info = new FileInfo(filename);
                DateTime mofify = info.CreationTime;

                using (var reader = System.IO.File.OpenText(filename))
                {
                    reader.ReadToEnd();
                    //Console.WriteLine(reader.ReadToEnd());
                }
            }
        }
    }

}
