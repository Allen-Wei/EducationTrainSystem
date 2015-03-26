using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    public class TrainController : ApiController
    {
        private EducationTrain model = new EducationTrain();
        public TrainController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public IEnumerable<Train> Get()
        {
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", model.Trains.LongCount().ToString());
            return model.Trains;
        }
        public IEnumerable<Train> Get(int take, int skip)
        {
            var total = model.Trains.LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total)/Convert.ToDouble(take));

            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.Trains.Skip(skip).Take(take);
        }
        public Train Get(int id)
        {
            return model.Trains.FirstOrDefault(t=> t.Id == id);
        }
        public Train Get(string name)
        {
            
            return model.Trains.FirstOrDefault(t => t.Name == name);
        }
        public Train Put(Train entity)
        {
            model.ObjectTrackingEnabled = true;
            model.Trains.InsertOnSubmit(entity);
            model.SubmitChanges();
            return entity;
        }
        public bool Post(Train entity)
        {
            model.ObjectTrackingEnabled = true;
            var query = this.Get(entity.Id);
            if (query == null) return false;
            query.Name = entity.Name;
            query.Description = entity.Description;
            query.Category = entity.Category;
            model.SubmitChanges();
            return true;
        }
    }
}
