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
    //[Authorize(Roles = "sales")]

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
        public IEnumerable<Object> Get(int take, int skip)
        {
            var total = model.Registrations.LongCount();
            var pages = Math.Ceiling((Convert.ToDouble(total) / Convert.ToDouble(take)));
            HttpContext.Current.Response.AddHeader("X-HeyHey-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-HeyHey-Pages", pages.ToString());
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.RegTrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.RegTrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.RegTrainId equals school.Gid into st
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         })
                         .Skip(skip)
                         .Take(take)
                         .ToList()
                         .Select(obj => new
                         {
                             Registration = obj.reg,
                             RegUser = obj.ru.FirstOrDefault(),
                             EduTrain = obj.et.FirstOrDefault(),
                             CertificationTrain = obj.ct.FirstOrDefault(),
                             SchoolTrain = obj.st.FirstOrDefault()
                         });


            return query;
        }


        [AllowAnonymous]
        public object GetDetail(Guid gid)
        {
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.RegTrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.RegTrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.RegTrainId equals school.Gid into st
                         where reg.Gid == gid
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         }).Select(obj => new
                         {
                             Registration = obj.reg,
                             RegUser = obj.ru.FirstOrDefault(),
                             EduTrain = obj.et.FirstOrDefault(),
                             CertificationTrain = obj.ct.FirstOrDefault(),
                             SchoolTrain = obj.st.FirstOrDefault()
                         })
                        .FirstOrDefault();
            return query;
        }

        [AllowAnonymous]
        public Registration Get(Guid key)
        {
            return model.Registrations.FirstOrDefault(r => r.Gid == key);
        }

        public bool Put([FromBody]RegPutEntity entity, [FromUri] int level)
        {
            model.ObjectTrackingEnabled = true;

            if (level == 3)
            {
                if (entity.TrainType == Registration.Train.EduTrains)
                    entity.Reg.RegTrainId = this.PutEduTrain(entity.Edu);
                if (entity.TrainType == Registration.Train.CertificationTrains)
                    entity.Reg.RegTrainId = this.PutCertTrain(entity.Cert);
                if (entity.TrainType == Registration.Train.SchoolTrains)
                    entity.Reg.RegTrainId = this.PutSchoolTrain(entity.School);
                entity.Reg.RegTrainName = entity.TrainType.ToString();
            }
            if (level == 2 || level == 3)
            {
                entity.Reg.RegUserId = this.PutUser(entity.User);
            }
            entity.Reg.GenerateDate = DateTime.Now;
            entity.Reg.Gid = Guid.NewGuid();
            model.Registrations.InsertOnSubmit(entity.Reg);
            model.SubmitChanges();
            return true;
        }
        private Guid PutUser(RegUser user)
        {
            user.Gid = Guid.NewGuid();
            model.RegUsers.InsertOnSubmit(user);
            model.SubmitChanges();
            return user.Gid;
        }
        private Guid PutEduTrain(EduTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.EduTrains.InsertOnSubmit(train);
            model.SubmitChanges();
            return train.Gid;
        }
        private Guid PutCertTrain(CertificationTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.CertificationTrains.InsertOnSubmit(train);
            model.SubmitChanges();
            return train.Gid;
        }
        private Guid PutSchoolTrain(SchoolTrain train)
        {
            model.SchoolTrains.InsertOnSubmit(train);
            model.SubmitChanges();
            return train.Gid;
        }
        public Registration Post(Registration registration)
        {
            model.ObjectTrackingEnabled = true;
            var reg = model.Registrations.FirstOrDefault(r => r.Id == registration.Id);
            if (reg == null) { return null; }

            reg.ReceiptNumber = registration.ReceiptNumber;
            reg.Price = registration.Price;
            reg.Payee = registration.Payee;
            reg.Note = registration.Note;

            model.SubmitChanges();
            return reg;
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


        public class RegPutEntity
        {
            public Registration.Train TrainType { get; set; }
            public Registration Reg { get; set; }
            public RegUser User { get; set; }
            public EduTrain Edu { get; set; }
            public CertificationTrain Cert { get; set; }
            public SchoolTrain School { get; set; }
        }
    }
}
