using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using Education.Models;

namespace Education.APIv1
{
    [Authorize(Roles = "sales")]

    public class RegistrationController : ApiController
    {
        private EducationModel model = new EducationModel();

        public IEnumerable<Registration> Get(int take, int skip)
        {
            var total = model.Registrations.LongCount();
            var pages = Math.Ceiling((Convert.ToDouble(total) / Convert.ToDouble(take)));
            HttpContext.Current.Response.AddHeader("X-HeyHey-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-HeyHey-Pages", pages.ToString());
            return model.Registrations.Skip(skip).Take(take).ToList();
        }

        public Registration Get(int id)
        {
            var reg = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) return null;
            return reg;
        }

        [AllowAnonymous]
        public Registration Get(Guid key)
        {
            return model.Registrations.FirstOrDefault(r => r.KeyId == key);
        }
        public Registration Put(Registration registration)
        {
            registration.GenerateDate = DateTime.Now;
            registration.KeyId = Guid.NewGuid();
            model.Registrations.InsertOnSubmit(registration);
            model.SubmitChanges();
            return registration;
        }

        public Registration Post(Registration registration)
        {
            var reg = model.Registrations.FirstOrDefault(r => r.Id == registration.Id);
            if (reg == null) { return null; }
            reg.RegistrateDate = registration.RegistrateDate;
            reg.StudentName = registration.StudentName;
            reg.Gender = registration.Gender;
            reg.Phone = registration.Phone;
            reg.Phone2 = registration.Phone2;
            reg.HomeAddress = registration.HomeAddress;
            reg.LiveAddress = registration.LiveAddress;

            reg.ReceiptNumber = registration.ReceiptNumber;
            reg.Price = registration.Price;
            reg.Payee = registration.Payee;
            reg.RegistrationAddress = registration.RegistrationAddress;
            reg.Note = registration.Note;
            reg.CourseCategoryName = registration.CourseCategoryName;
            reg.CourseName = registration.CourseName;

            reg.CurrentCollege = registration.CurrentCollege;
            reg.CurrentGrade = registration.CurrentGrade;
            reg.RegistrateCollege = registration.RegistrateCollege;
            reg.EducationDegree = registration.EducationDegree;
            model.SubmitChanges();
            return reg;
        }

        public bool Delete(int id)
        {

            var reg = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) { return false; }
            model.Registrations.DeleteOnSubmit(reg);
            model.SubmitChanges();
            return true;
        }
    }
}
