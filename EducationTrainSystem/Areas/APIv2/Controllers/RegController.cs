using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Education.Models;

namespace Education.Areas.APIv2.Controllers
{
    public class RegController : Controller
    {
        private EducationModel model = new EducationModel();
        public JsonResult Apply(Registration reg)
        {
            reg.KeyId = Guid.NewGuid();
            reg.GenerateDate = DateTime.Now;
            reg.Agent = " ";
            reg.Payee = " ";
            reg.Price = 0;
            reg.ReceiptNumber = "";
            reg.Confirm = false;
            model.Registrations.InsertOnSubmit(reg);
            model.SubmitChanges();
            return Json(reg);
        }

    }
}
