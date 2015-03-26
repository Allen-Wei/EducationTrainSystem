using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.Areas.APIv2.Controllers
{
    public class RegController : Controller
    {
        public JsonResult Apply(RegEntity reg)
        {
            var success = reg.Apply();
            return Json(new { success, gid = reg.Reg.Gid });
        }

    }
}
