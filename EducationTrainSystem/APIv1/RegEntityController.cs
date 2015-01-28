using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using EducationTrainSystem.Models;
using EducationTrainSystem.Library;

namespace EducationTrainSystem.APIv1
{
    public class RegEntityController : ApiController
    {
        public class RegEntity
        {
            public Registration.TrainsCategory TrainCategory { get; set; }
            public Registration Reg { get; set; }
            public RegUser User { get; set; }
            public EduTrain Edu { get; set; }
            public CertificationTrain Cert { get; set; }
            public SchoolTrain School { get; set; }
        }
        private EducationTrain model = new EducationTrain();

        private RegEntityController()
        {
            model.ObjectTrackingEnabled = false;
        }

        public RegEntity Get(int id)
        {
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
                         where reg.Id == id
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         }).Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         })
                         .FirstOrDefault();
            return query;
        }

        [AllowAnonymous]
        public RegEntity GetDetail(Guid gid)
        {
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
                         where reg.Gid == gid
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         }).Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         })
                        .FirstOrDefault();
            return query;
        }


        public IEnumerable<RegEntity> Get(int take, int skip)
        {
            var total = model.Registrations.LongCount();
            var pages = Math.Ceiling((Convert.ToDouble(total) / Convert.ToDouble(take)));
            HttpContext.Current.Response.AddHeader("X-HeyHey-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-HeyHey-Pages", pages.ToString());
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
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
                         .Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         });


            return query;
        }


        public bool Put(RegEntity entity)
        {
            model.ObjectTrackingEnabled = true;

            if (entity.TrainCategory == Registration.TrainsCategory.EduTrains)
                entity.Reg.TrainId = this.PutTrain(entity.Edu);
            if (entity.TrainCategory == Registration.TrainsCategory.CertificationTrains)
                entity.Reg.TrainId = this.PutTrain(entity.Cert);
            if (entity.TrainCategory == Registration.TrainsCategory.SchoolTrains)
                entity.Reg.TrainId = this.PutTrain(entity.School);
            entity.Reg.TrainCategory = entity.TrainCategory.ToString();

            entity.Reg.RegUserId = this.PutUser(entity.User);
            entity.Reg.GenerateDate = DateTime.Now;
            entity.Reg.Gid = Guid.NewGuid();
            model.Registrations.InsertOnSubmit(entity.Reg);
            model.SubmitChanges();
            return true;
        }

        //update 
        public bool Post(RegEntity entity)
        {
            model.ObjectTrackingEnabled = true;
            var reg = model.Registrations.FirstOrDefault(r => r.Id == entity.Reg.Id || r.Gid == entity.Reg.Gid);
            if (reg == null) return false;

            reg.SetValuesInclude(
                entity.Reg,
                "Address",
                "ReceiptNumber",
                "Agent",
                "Note",
                "Confirmed",
                "Note",
                "Payee",
                "Price");

            var user = model.RegUsers.FirstOrDefault(ru => ru.Gid == reg.RegUserId);
            if (user == null) reg.RegUserId = this.PutUser(entity.User);
            else this.PostUser(entity.User);

            var action = "update";
            var newTrainId = Guid.NewGuid();
            if (entity.TrainCategory == Registration.TrainsCategory.EduTrains)
            {
                if (!this.PostTrain(entity.Edu))
                {
                    action = "add";
                    newTrainId = this.PutTrain(entity.Edu);
                }
            }
            if (entity.TrainCategory == Registration.TrainsCategory.CertificationTrains)
            {
                if (!this.PostTrain(entity.Cert))
                {
                    action = "add";
                    newTrainId = this.PutTrain(entity.Cert);
                }
            }
            if (entity.TrainCategory == Registration.TrainsCategory.SchoolTrains)
            {
                if (!this.PostTrain(entity.School))
                {
                    action = "add";
                    newTrainId = this.PutTrain(entity.School);
                }
            }
            if (action == "add")
            {
                Guid oldTrainId;
                if (reg.TrainId != null && Guid.TryParse(reg.TrainId.ToString(), out oldTrainId))
                {
                    if (reg.TrainCategory == Registration.TrainsCategory.EduTrains.ToString()) this.DeleteEduTrain(oldTrainId);
                    if (reg.TrainCategory == Registration.TrainsCategory.CertificationTrains.ToString()) this.DeleteCertTrain(oldTrainId);
                    if (reg.TrainCategory == Registration.TrainsCategory.SchoolTrains.ToString()) this.DeleteSchoolTrain(oldTrainId);
                }
                reg.TrainId = newTrainId;
                reg.TrainCategory = entity.TrainCategory.ToString();
            }

            model.SubmitChanges();
            return true;
        }

        public bool Delete(int id)
        {
            model.ObjectTrackingEnabled = true;
            var reg = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) return false;
            model.Registrations.DeleteOnSubmit(reg);
            model.SubmitChanges();
            return true;
        }

        #region Put User, EduTrain, CertTrain
        private Guid PutUser(RegUser user)
        {
            user.Gid = Guid.NewGuid();
            model.RegUsers.InsertOnSubmit(user);
            model.SubmitChanges();
            return user.Gid;
        }
        private Guid PutTrain(EduTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.EduTrains.InsertOnSubmit(train);
            model.SubmitChanges();
            return train.Gid;
        }
        private Guid PutTrain(CertificationTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.CertificationTrains.InsertOnSubmit(train);
            model.SubmitChanges();
            return train.Gid;
        }
        private Guid PutTrain(SchoolTrain train)
        {
            model.SchoolTrains.InsertOnSubmit(train);
            model.SubmitChanges();
            return train.Gid;
        }
        #endregion

        #region Post
        private bool PostUser(RegUser entity)
        {
            var query = model.RegUsers.FirstOrDefault(ru => ru.Gid == entity.Gid);
            if (query == null) return false;
            query.SetValuesExclude(entity, "Gid", "RegDate");
            return true;
        }

        private bool PostTrain(EduTrain entity)
        {
            var query = model.EduTrains.FirstOrDefault(t => t.Gid == entity.Gid);
            if (query == null) return false;
            query.SetValuesExclude(entity, "Gid");
            return true;
        }

        private bool PostTrain(CertificationTrain entity)
        {
            var query = model.CertificationTrains.FirstOrDefault(t => t.Gid == entity.Gid);
            if (query == null) return false;
            query.SetValuesExclude(entity, "Gid");
            return true;
        }

        private bool PostTrain(SchoolTrain entity)
        {
            var query = model.SchoolTrains.FirstOrDefault(t => t.Gid == entity.Gid);
            if (query == null) return false;
            query.SetValuesExclude(entity, "Gid");
            return true;
        }
        #endregion

        #region
        private bool DeleteEduTrain(Guid id)
        {
            var query = model.EduTrains.FirstOrDefault(t => t.Gid == id);
            if (query == null) return false;
            model.EduTrains.DeleteOnSubmit(query);
            return true;
        }

        private bool DeleteCertTrain(Guid id)
        {
            var query = model.CertificationTrains.FirstOrDefault(t => t.Gid == id);
            if (query == null) return false;
            model.CertificationTrains.DeleteOnSubmit(query);
            return true;
        }

        private bool DeleteSchoolTrain(Guid id)
        {
            var query = model.SchoolTrains.FirstOrDefault(t => t.Gid == id);
            if (query == null) return false;
            model.SchoolTrains.DeleteOnSubmit(query);
            return true;
        }

        #endregion
    }
}
