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
        public CourseController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public IEnumerable<Course> Get()
        {
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", model.Courses.LongCount().ToString());
            return model.Courses;
        }
        public IEnumerable<Course> Get(int take, int skip)
        {
            var total = model.Courses.LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total)/Convert.ToDouble(take));

            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.Courses.Skip(skip).Take(take);
        }
        public IEnumerable<Course> GetByTrain(int train) {
            return model.Courses.Where(c => c.TrainId == train);
        }
        public Course Get(int id)
        {
            return model.Courses.FirstOrDefault(c => c.Id == id);
        }
        public Course Get(string name)
        {
            return model.Courses.FirstOrDefault(c => c.Name == name);
        }
        public Course Put(Course entity)
        {
            model.ObjectTrackingEnabled = true;
            model.Courses.InsertOnSubmit(entity);
            model.SubmitChanges();
            return entity;
        }
        public bool Post(Course entity)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.Courses.FirstOrDefault(c => c.Id == entity.Id);
            if (query == null) return false;
            query.Name = entity.Name;
            query.Description = entity.Description;
            model.SubmitChanges();
            return true;
        }
    }
}
