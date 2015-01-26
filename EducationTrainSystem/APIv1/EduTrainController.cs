using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    public class EduTrainController : ApiController
    {
        private EducationTrain model = new EducationTrain();
        public EduTrainController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public EduTrain Get(Guid id)
        {
            return model.EduTrains.FirstOrDefault(et => et.Gid == id);
        }
        public IEnumerable<EduTrain> Get(int skip, int take)
        {
            return model.EduTrains.Skip(skip).Take(take);
        }
        public EduTrain Put(EduTrain entity)
        {
            model.ObjectTrackingEnabled = true;
            model.EduTrains.InsertOnSubmit(entity);
            model.SubmitChanges();
            return entity;
        }
        public bool Post(EduTrain entity)
        {
            model.ObjectTrackingEnabled = true;
            var query = this.Get(entity.Gid);
            if (query == null) return false;
            query.Course = entity.Course;
            query.RegCollege = entity.RegCollege;
            query.RegMajor = entity.RegMajor;
            query.EduType = entity.EduType;
            query.CurrentCollege = entity.CurrentCollege;
            query.CurrentGrade = entity.CurrentGrade;
            return true;
        }
    }
}
