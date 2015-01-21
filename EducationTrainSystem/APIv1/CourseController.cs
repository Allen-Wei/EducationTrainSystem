using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;


namespace EducationTrainSystem.APIv1
{
    public class CourseController : ApiController
    {
        private EducationTrain model = new EducationTrain();
        public IEnumerable<Course> GetCourses(int train)
        {
            return model.Courses.Where(c => c.TrainId == train);
        }
        public Course GetCourse(string name)
        {
            return model.Courses.FirstOrDefault(c => c.Name == name);
        }
    }
}
