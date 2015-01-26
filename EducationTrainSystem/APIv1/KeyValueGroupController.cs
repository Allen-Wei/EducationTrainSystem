using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    public class KeyValueGroupController : ApiController
    {
        private EducationTrain model = new EducationTrain();
        public KeyValueGroupController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public IEnumerable<KeyValueGroup> Get()
        {
            return model.KeyValueGroups;
        }
        public KeyValueGroup Get(int id)
        {
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", model.KeyValueGroups.LongCount().ToString());
            return model.KeyValueGroups.FirstOrDefault(kvg => kvg.Id == id);
        }
        public IEnumerable<KeyValueGroup> Get(int take, int skip)
        {
            var total = model.KeyValueGroups.LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValueGroups.Skip(skip).Take(take);
        }
        public KeyValueGroup Put(KeyValueGroup entity)
        {
            model.ObjectTrackingEnabled = true;
            model.KeyValueGroups.InsertOnSubmit(entity);
            model.SubmitChanges();
            return entity;
        }
        public bool Post(KeyValueGroup entity)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Id == entity.Id);
            if (query == null) { return false; }
            query.Name = entity.Name;
            query.Description = entity.Description;
            query.Category = entity.Category;
            model.SubmitChanges();
            return true;
        }
        public bool Delete(int id)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Id == id);
            if (query == null) { return false; }
            model.KeyValueGroups.DeleteOnSubmit(query);
            model.SubmitChanges();
            return true;
        }
    }
}
