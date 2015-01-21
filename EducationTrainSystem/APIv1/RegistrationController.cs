using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    [Authorize(Roles = "sales")]

    public class RegistrationController : ApiController
    {
        private EducationTrain model = new EducationTrain();

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
            return model.Registrations.FirstOrDefault(r => r.GId == key);
        }
        public Registration Put(Registration registration)
        {
            registration.GenerateDate = DateTime.Now;
            registration.GId = Guid.NewGuid();
            model.Registrations.InsertOnSubmit(registration);
            model.SubmitChanges();
            return registration;
        }

        public Registration Post(Registration registration)
        {
            var reg = model.Registrations.FirstOrDefault(r => r.Id == registration.Id);
            if (reg == null) { return null; }
            //reg.RegistrateDate = registration.RegistrateDate;
            //reg.StudentName = registration.StudentName;
            //reg.Gender = registration.Gender;
            //reg.Phone = registration.Phone;
            //reg.Phone2 = registration.Phone2;
            //reg.HomeAddress = registration.HomeAddress;
            //reg.LiveAddress = registration.LiveAddress;

            reg.ReceiptNumber = registration.ReceiptNumber;
            reg.Price = registration.Price;
            reg.Payee = registration.Payee;
            reg.Note = registration.Note;

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
