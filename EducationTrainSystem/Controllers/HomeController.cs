using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationTrainSystem.Models;
using EducationTrainSystem.Library;
using System.Linq.Dynamic;
using System.Data.Linq;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;

namespace EducationTrainSystem.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //public FileResult Export()
        //{
        //    byte[] result;
        //    using (ExcelPackage pack = new ExcelPackage())
        //    {
        //        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("BaseInformation");
        //        using (EducationTrain model = new EducationTrain())
        //        {
        //            ws.Cells["A1"].Value = "#";
        //            ws.Cells["B1"].Value = "唯一编号";
        //            ws.Cells["C1"].Value = "报名日期";
        //            ws.Cells["D1"].Value = "代理人";
        //            ws.Cells["E1"].Value = "付款人";
        //            ws.Cells["F1"].Value = "是否已确认";
        //            ws.Cells["G1"].Value = "价格";
        //            ws.Cells["H1"].Value = "备注";

        //            ws.Cells["A2"].LoadFromCollection(model.Registrations.Select(r => new {
        //                r.Id, 
        //                r.Gid,
        //                r.GenerateDate,
        //                r.Agent,
        //                r.Payee,
        //                Confirm = r.Confirmed ? "是" : "否",
        //                r.Price,
        //                r.Note }).ToArray());
        //        }
        //        ws.Cells.AutoFitColumns();
        //        result = pack.GetAsByteArray();
        //    }
        //    Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
        //    return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //}
    }
}
