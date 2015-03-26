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


        public RegistrationController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public Registration Get(int id)
        {
            return model.Registrations.FirstOrDefault(r => r.Id == id);
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


    }
}
