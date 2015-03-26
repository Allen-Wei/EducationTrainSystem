using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;


namespace EducationTrainSystem.APIv1
{
    public class KeyValueMatchController : ApiController
    {
        private EducationTrain model = new EducationTrain();
        public KeyValueMatchController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public IEnumerable<KeyValueMatch> GetAll() {
            return model.KeyValueMatches;
        }
        public KeyValueMatch GetMatch(int id)
        {
            return model.KeyValueMatches.FirstOrDefault(kvm => kvm.Id == id);
        }
        public IEnumerable<KeyValueMatch> Get(int skip, int take)
        {
            var total = model.KeyValueMatches.LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValueMatches.Skip(skip).Take(take);
        }
     
        public KeyValueMatch Put(KeyValueMatch entity)
        {
            model.ObjectTrackingEnabled = true;
            model.KeyValueMatches.InsertOnSubmit(entity);
            model.SubmitChanges();
            return entity;
        }
        public bool Post(KeyValueMatch entity)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.KeyValueMatches.FirstOrDefault(kvm => kvm.Id == entity.Id);
            if (query == null) { return false; }
            query.ValueId = entity.ValueId;
            query.GroupId = entity.GroupId;
            model.SubmitChanges();
            return true;
        }
        public bool Delete(int id)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.KeyValueMatches.FirstOrDefault(kvm => kvm.Id == id);
            if (query == null) { return false; }
            model.KeyValueMatches.DeleteOnSubmit(query);
            model.SubmitChanges();
            return true;
        }
        public bool Delete(int group, int value)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.KeyValueMatches.FirstOrDefault(kvm => kvm.GroupId == group && kvm.ValueId == value);
            if (query == null) { return false; }
            model.KeyValueMatches.DeleteOnSubmit(query);
            model.SubmitChanges();
            return true;
        }
    }
}
