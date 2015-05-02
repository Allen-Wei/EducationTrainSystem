using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.Controllers
{
    public class TaskController : Controller
    {

        private EducationTrain model = new EducationTrain();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(model.Registrations);
        }

        public ActionResult Reg(int id)
        {
            var result = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (result == null) { return HttpNotFound(); }
            ViewBag.Trains = new List<SelectListItem>()
            {
                new SelectListItem() {Value = "EduTrain", Text = "学历教育"},
                new SelectListItem {Value = "SchoolTrain", Text = "中小学辅导"},
                new SelectListItem() {Value = "CertTrain", Text = "资格证培训"}
            }.AsEnumerable();
            ViewBag.Addresses = new SelectList(model.KeyValues.Where(kv => kv.Mark == "报名地点"), "Name", "Name", result.Address);
            return View(result);
        }

        [HttpPost]
        public ActionResult Reg()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Registration reg = new Registration();
            TryUpdateModel(reg);
            Response.AddHeader("X-Registration", js.Serialize(reg));

            var query = model.Registrations.FirstOrDefault(r => r.Id == reg.Id);
            if (query == null) { return HttpNotFound(); }
            query.Note = reg.Note;
            query.Address = reg.Address;
            model.SubmitChanges();

            ViewBag.Trains = new List<SelectListItem>()
            {
                new SelectListItem() {Value = "EduTrain", Text = "学历教育"},
                new SelectListItem {Value = "SchoolTrain", Text = "中小学辅导"},
                new SelectListItem() {Value = "CertTrain", Text = "资格证培训"}
            }.AsEnumerable();
            ViewBag.Addresses = new SelectList(model.KeyValues.Where(kv => kv.Mark == "报名地点"), "Name", "Name", query.Address);
            return View(reg);
        }

        public ActionResult People()
        {
            var person = new Person();
            return View(person);
        }

        [HttpPost]
        public ActionResult People(Person p)
        {
            return View(p);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="E-mail")]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
    }
}
