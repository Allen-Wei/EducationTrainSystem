using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EducationTrainSystem.Models;
using EducationTrainSystem.Library;


namespace EducationTrainSystem.Models
{
    public class RegEntity
    {

        public Registration.TrainsCategory TrainCategory { get; set; }
        public Registration Reg { get; set; }
        public RegUser User { get; set; }
        public EduTrain Edu { get; set; }
        public CertificationTrain Cert { get; set; }
        public SchoolTrain School { get; set; }


        private EducationTrain model = new EducationTrain();



        public bool Add()
        {

            if (this.TrainCategory == Registration.TrainsCategory.EduTrains) this.Reg.TrainId = this.PutTrain(this.Edu);
            if (this.TrainCategory == Registration.TrainsCategory.CertificationTrains) this.Reg.TrainId = this.PutTrain(this.Cert);
            if (this.TrainCategory == Registration.TrainsCategory.SchoolTrains) this.Reg.TrainId = this.PutTrain(this.School);

            this.Reg.TrainCategory = this.TrainCategory.ToString();

            this.Reg.RegUserId = this.PutUser(this.User);
            this.Reg.GenerateDate = DateTime.Now;

            this.Reg.Gid = Guid.NewGuid();
            this.Reg.GenerateDate = DateTime.Now;
            model.Registrations.InsertOnSubmit(this.Reg);
            model.SubmitChanges();
            return true;
        }

        //update 
        public bool Update(RegEntity entity)
        {
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
            }
            reg.TrainCategory = entity.TrainCategory.ToString();

            model.SubmitChanges();
          
            return true;
        }

        public bool Delete(int id)
        {
            var reg = model.Registrations.FirstOrDefault(r => r.Id == id);
            if (reg == null) return false;

            var trainId = Guid.NewGuid();
            if (reg.TrainId != null && Guid.TryParse(reg.TrainId.ToString(), out trainId))
            {
                if (reg.TrainCategory == Registration.TrainsCategory.EduTrains.ToString()) this.DeleteEduTrain(trainId);
                if (reg.TrainCategory == Registration.TrainsCategory.CertificationTrains.ToString()) this.DeleteCertTrain(trainId);
                if (reg.TrainCategory == Registration.TrainsCategory.SchoolTrains.ToString()) this.DeleteSchoolTrain(trainId);
            }

            var userId = Guid.NewGuid();
            if (reg.RegUserId != null && Guid.TryParse(reg.RegUserId.ToString(), out userId))
            {
                var queryUser = model.RegUsers.FirstOrDefault(ru => ru.Gid == userId);
                if (queryUser != null) { model.RegUsers.DeleteOnSubmit(queryUser); }
            }

            model.Registrations.DeleteOnSubmit(reg);
            model.SubmitChanges();
            return true;
        }

        public bool Apply()
        {
            this.Reg.Initial();
            this.Reg.Confirmed = false;
            this.Reg.ReceiptNumber = " ";
            this.Reg.Price = 0;
            this.Reg.Agent = " ";
            this.Reg.Payee = " ";
            this.Reg.Address = " ";

            this.User.Gid = Guid.NewGuid();
            this.User.RegDate = DateTime.Now;
            this.Reg.RegUserId = this.User.Gid;

            this.Reg.TrainCategory = this.TrainCategory.ToString();
            if (this.TrainCategory == Registration.TrainsCategory.EduTrains) this.Reg.TrainId = this.PutTrain(this.Edu);
            if (this.TrainCategory == Registration.TrainsCategory.CertificationTrains) this.Reg.TrainId = this.PutTrain(this.Cert);
            if (this.TrainCategory == Registration.TrainsCategory.SchoolTrains) this.Reg.TrainId = this.PutTrain(this.School);

            model.Registrations.InsertOnSubmit(this.Reg);
            model.RegUsers.InsertOnSubmit(this.User);
            model.SubmitChanges();
            return true;
        }


        #region Put User, EduTrain, CertTrain
        private Guid PutUser(RegUser user)
        {
            user.Gid = Guid.NewGuid();
            model.RegUsers.InsertOnSubmit(user);
            return user.Gid;
        }
        private Guid PutTrain(EduTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.EduTrains.InsertOnSubmit(train);
            return train.Gid;
        }
        private Guid PutTrain(CertificationTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.CertificationTrains.InsertOnSubmit(train);
            return train.Gid;
        }
        private Guid PutTrain(SchoolTrain train)
        {
            train.Gid = Guid.NewGuid();
            model.SchoolTrains.InsertOnSubmit(train);
            foreach (var sub in train.SchoolSubjects)
            {
                sub.TrainId = train.Gid;
            }
            model.SchoolSubjects.InsertAllOnSubmit(train.SchoolSubjects);
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
            query.RegStage = entity.RegStage;
            model.SchoolSubjects.DeleteAllOnSubmit(model.SchoolSubjects.Where(ss=>ss.TrainId == entity.Gid));
            foreach (var sub in entity.SchoolSubjects)
            {
                sub.TrainId = entity.Gid;
            }
            model.SchoolSubjects.InsertAllOnSubmit(entity.SchoolSubjects);
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
            model.SchoolSubjects.DeleteAllOnSubmit(model.SchoolSubjects.Where(ss=>ss.TrainId == query.Gid));
            model.SchoolTrains.DeleteOnSubmit(query);
            return true;
        }

        #endregion
    }
}