using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    public class CourseCategoryController : ApiController
    {
        private EducationTrain model = new EducationTrain();
        public IEnumerable<Train> Get()
        {
            return model.Trains;
        }
        public Train Get(string id)
        {
            return model.Trains.FirstOrDefault(cc => cc.Name == id);
        }
    }
}
