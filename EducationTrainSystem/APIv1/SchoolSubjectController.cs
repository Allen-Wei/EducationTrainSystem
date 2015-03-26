using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    public class SchoolSubjectController : ApiController
    {
        private EducationTrain model;

        public SchoolSubjectController()
        {
            model = new EducationTrain();
            model.ObjectTrackingEnabled = false;
        }

        public SchoolSubject Get(int id)
        {
            return model.SchoolSubjects.FirstOrDefault(ss => ss.Id == id);
        }

        public IEnumerable<SchoolSubject> Get(Guid trainId)
        {
            return model.SchoolSubjects.Where(ss => ss.TrainId == trainId);
        }

    }
}
