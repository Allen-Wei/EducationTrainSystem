using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.Controllers
{
    public class EducationController : Controller
    {
        private EducationTrain model = new EducationTrain();

        [Authorize(Roles = "sales")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "sales")]
        public ActionResult Receipt(int id)
        {
            var reg = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) { Response.Redirect("/"); }
            return View(reg);
        }

        public ActionResult Apply()
        {
            return View();
        }

        public ActionResult ApplySuccess()
        {
            return View();
        }

        public string GenerateManifest()
        {
            StringBuilder manifest = new StringBuilder();
            var files = GetFiles("fonts");
            files.AddRange(GetFiles("Styles"));
            files.AddRange(GetFiles("Styles/images"));
            files.AddRange(GetFiles("Partials/Education"));
            files.AddRange(GetFiles("Scripts/angular"));
            files.AddRange(GetFiles("Vendors"));
            files.ForEach(f => manifest.AppendFormat("{0}<br />", f));
            return manifest.ToString();
        }

        private List<string> GetFiles(string folderName)
        {
            var path = Server.MapPath(String.Format("~/{0}", folderName));
            var dir = new DirectoryInfo(path);
            return dir.GetFiles().Select(f => String.Format("/Education/{0}/{1}", folderName, f.Name)).ToList();
        }

        public string CreateDatabase()
        {
            if (model.DatabaseExists()) return "database is existed.";
            model.CreateDatabase();
            return "success";
        }

      
        public string InitialData()
        {
            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/Content/data.sql"));
            lines.Where(line => !String.IsNullOrWhiteSpace(line) && !line.StartsWith("--")).All(line =>
            {
                var result = model.ExecuteCommand(line);
                return true;
            });
            return "success";
        }
    }
}
