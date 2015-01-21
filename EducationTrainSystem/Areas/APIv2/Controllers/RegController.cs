using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationTrainSystem.Models;

namespace Education.Areas.APIv2.Controllers
{
    public class RegController : Controller
    {
        private EducationTrain model = new EducationTrain();
        public JsonResult Apply(Registration reg)
        {
            reg.GId = Guid.NewGuid();
            reg.GenerateDate = DateTime.Now;
            reg.Agent = " ";
            reg.Payee = " ";
            reg.Price = 0;
            reg.ReceiptNumber = "";
            reg.Confirmed = false;
            model.Registrations.InsertOnSubmit(reg);
            model.SubmitChanges();
            return Json(reg);
        }

    }
}
