using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EducationTrainSystem.Models;
using EducationTrainSystem.ViewModels;
using System.Linq.Dynamic;
using OfficeOpenXml;

namespace EducationTrainSystem.APIv1
{
    [Authorize(Roles = "sales")]

    public class RegistrationController : ApiController
    {
        private EducationTrain model = new EducationTrain();


        public RegistrationController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public Registration Get(int id)
        {
            return model.Registrations.FirstOrDefault(r => r.Id == id);
        }

        [HttpPost]
        [Route("apiv1/Registration/Query")]
        public IEnumerable<Registration> Query(QueryParams query)
        {
            var total = model.Registrations.Where(query.Condition, query.Parameters).LongCount();
            var pages = Math.Ceiling((Convert.ToDouble(total) / Convert.ToDouble(query.Take)));
            HttpContext.Current.Response.AddHeader("X-HeyHey-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-HeyHey-Pages", pages.ToString());

            var registrations = model.Registrations
                .Where(query.Condition, query.Parameters)
                .OrderBy(query.Order)
                .Skip(query.Skip)
                .Take(query.Take)
                .ToList();

            var userId = registrations.Select(r => r.RegUserId).ToList();
            var users = model.RegUsers.Where(ru => userId.Contains(ru.Gid)).ToList();
            var eduTrainIds = registrations.Where(r => r.TrainCategory == Registration.TrainsCategory.EduTrains.ToString()).Select(r => r.TrainId).ToList();
            var eduTrains = model.EduTrains.Where(et => eduTrainIds.Contains(et.Gid)).ToList();
            var certTrainIds = registrations.Where(r => r.TrainCategory == Registration.TrainsCategory.CertificationTrains.ToString()).Select(r => r.TrainId).ToList();
            var certTrains = model.CertificationTrains.Where(ct => certTrainIds.Contains(ct.Gid)).ToList();
            var schoolTrainIds = registrations.Where(r => r.TrainCategory == Registration.TrainsCategory.SchoolTrains.ToString()).Select(r => r.TrainId).ToList();
            var schoolTrains = model.SchoolTrains.Where(st => schoolTrainIds.Contains(st.Gid)).ToList();

            registrations.ForEach(r =>
            {
                r.User = users.FirstOrDefault(u => u.Gid == r.RegUserId);
                if (r.TrainCategory == Registration.TrainsCategory.CertificationTrains.ToString())
                    r.Train = certTrains.FirstOrDefault(ct => ct.Gid == r.TrainId);
                if (r.TrainCategory == Registration.TrainsCategory.EduTrains.ToString())
                    r.Train = eduTrains.FirstOrDefault(et => et.Gid == r.TrainId);
                if (r.TrainCategory == Registration.TrainsCategory.SchoolTrains.ToString())
                    r.Train = schoolTrains.FirstOrDefault(et => et.Gid == r.TrainId);

            });
            return registrations;


        }





        [AllowAnonymous]
        public Registration Get(Guid key)
        {
            return model.Registrations.FirstOrDefault(r => r.Gid == key);
        }

        public IEnumerable<Registration> Get(int skip, int take)
        {
            return model.Registrations.Skip(skip).Take(take);
        }

        public Registration Put(Registration reg)
        {
            model.ObjectTrackingEnabled = true;
            model.Registrations.InsertOnSubmit(reg);
            model.SubmitChanges();
            return reg;
        }


        public bool Post(Registration registration)
        {
            model.ObjectTrackingEnabled = true;
            var reg = model.Registrations.FirstOrDefault(r => r.Id == registration.Id);
            if (reg == null) { return false; }

            reg.ReceiptNumber = registration.ReceiptNumber;
            reg.Price = registration.Price;
            reg.Payee = registration.Payee;
            reg.Note = registration.Note;
            reg.TrainCategory = registration.TrainCategory;
            reg.TrainId = registration.TrainId;

            model.SubmitChanges();
            return true;
        }

        public bool Delete(int id)
        {
            model.ObjectTrackingEnabled = true;
            var reg = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) { return false; }
            model.Registrations.DeleteOnSubmit(reg);
            model.SubmitChanges();
            return true;
        }

        [HttpGet]
        [Route("apiv1/Registration/Export")]
        public HttpResponseMessage Export()
        {

            byte[] result;
            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("BaseInformation");
                ws.Cells["A1"].Value = "#";
                ws.Cells["B1"].Value = "唯一编号";
                ws.Cells["C1"].Value = "报名日期";
                ws.Cells["D1"].Value = "代理人";
                ws.Cells["E1"].Value = "付款人";
                ws.Cells["F1"].Value = "是否已确认";
                ws.Cells["G1"].Value = "价格";
                ws.Cells["H1"].Value = "备注";

                ws.Cells["I1"].Value = "姓名";
                ws.Cells["J1"].Value = "性别";
                ws.Cells["K1"].Value = "户籍";
                ws.Cells["L1"].Value = "住址";
                ws.Cells["M1"].Value = "身份证";

                var query = (from reg in model.Registrations
                             join u in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals u.Gid into regu
                             select new { reg, regu }
                            ).Select(entity => new { entity.reg, user = entity.regu.FirstOrDefault() ?? new RegUser() });

                ws.Cells["A2"].LoadFromCollection(query.Select(r => new
                {
                    r.reg.Id,
                    r.reg.Gid,
                    GenerateDate = r.reg.GenerateDate.ToShortDateString(),
                    r.reg.Agent,
                    r.reg.Payee,
                    Confirm = r.reg.Confirmed ? "是" : "否",
                    r.reg.Price,
                    r.reg.Note,

                    r.user.Name,
                    Gender = r.user.Gender ? "男" : "女",
                    r.user.HomeAddress,
                    r.user.LiveAddress,
                    r.user.CardId

                }).ToArray());
                ws.Cells.AutoFitColumns();
                result = pack.GetAsByteArray();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(result);
            response.Content.Headers.Add("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
            response.Content.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;
        }

    }
}
