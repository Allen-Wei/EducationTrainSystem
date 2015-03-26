using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;
using System.Web;

namespace EducationTrainSystem.APIv1
{
    public class KeyValueController : ApiController
    {
        private EducationTrain model = new EducationTrain();

        public KeyValueController()
        {
            model.ObjectTrackingEnabled = false;
        }
        public IEnumerable<KeyValue> Get()
        {

            return model.KeyValues;
        }
        public KeyValue Get(int id)
        {
            return model.KeyValues.FirstOrDefault(kv => kv.Id == id);
        }

        public IEnumerable<KeyValue> Get(int take, int skip)
        {
            var total = model.KeyValues.LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValues.Skip(skip).Take(take);
        }

        //group relative
        public IEnumerable<KeyValue> GetByGroup(int gid)
        {
            var kvGroup = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Id == gid);
            if (kvGroup == null) return null;
            return model.KeyValues.Where(kv => model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id));
        }
        public IEnumerable<KeyValue> Get(int gid, int take, int skip)
        {
            var kvGroup = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Id == gid);
            if (kvGroup == null) return null;
            var total = model.KeyValues.Where(kv => model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id)).LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValues.Where(kv => model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id)).Skip(skip).Take(take);
        }
        public IEnumerable<KeyValue> GetExclude(int gidexclude, int take, int skip)
        {
            var kvGroup = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Id == gidexclude);
            if (kvGroup == null) return null;
            HttpContext.Current.Response.AddHeader("X-Edu-Description", String.Format("get {0} exlude keyvalues", kvGroup.Name));
            var total = model.KeyValues.Where(kv => !model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id)).LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValues
                .Where(kv => !model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id))
                .Skip(skip)
                .Take(take);
        }

        public IEnumerable<KeyValue> Get(string gname)
        {
            var kvGroup = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Name == gname);
            if (kvGroup == null) return null;
            return model.KeyValues.Where(kv => model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id));
        }
        public IEnumerable<KeyValue> Get(string gname, int take, int skip)
        {
            var kvGroup = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Name == gname);
            if (kvGroup == null) return null;
            var total = model.KeyValues.Where(kv => model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id)).LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValues.Where(kv => model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id)).Skip(skip).Take(take);
        }
        public IEnumerable<KeyValue> GetExclude(string gnameexclude, int take, int skip)
        {
            var kvGroup = model.KeyValueGroups.FirstOrDefault(kvg => kvg.Name == gnameexclude);
            if (kvGroup == null) return null;
            HttpContext.Current.Response.AddHeader("X-Edu-Description", String.Format("get {0} exlude keyvalues", kvGroup.Name));
            var total = model.KeyValues.Where(kv => !model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id)).LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValues
                .Where(kv => !model.KeyValueMatches.Where(kvm => kvm.GroupId == kvGroup.Id).Select(kvm => kvm.ValueId).Contains(kv.Id))
                .Skip(skip)
                .Take(take);
        }

        //mark relative
        public string[] GetMarks(bool getMark)
        {
            return model.KeyValues.Select(kv => kv.Mark).ToArray();
        }
        public IEnumerable<KeyValue> GetByMark(string mark)
        {
            Func<KeyValue, bool> condition = kv => kv.Mark == mark;

            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", model.KeyValues.Where(condition).LongCount().ToString());
            return model.KeyValues.Where(condition);
        }
        public IEnumerable<KeyValue> GetByMark(string mark, int take, int skip)
        {
            Func<KeyValue, bool> condition = kv => kv.Mark == mark;
            var total = model.KeyValues.Where(condition).LongCount();
            var pages = Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(take));
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Total", total.ToString());
            System.Web.HttpContext.Current.Response.AddHeader("X-Edu-Pages", pages.ToString());
            return model.KeyValues.Where(condition).Skip(skip).Take(take);
        }


        public KeyValue Put(KeyValue entity)
        {
            model.ObjectTrackingEnabled = true;
            model.KeyValues.InsertOnSubmit(entity);
            model.SubmitChanges();
            return entity;
        }
        public bool Post(KeyValue entity)
        {
            model.ObjectTrackingEnabled = true;
            var query = this.Get(entity.Id);
            if (query == null) { return false; }
            query.Name = entity.Name;
            query.Value = entity.Value;
            query.Description = entity.Description;
            query.Mark = entity.Mark;
            model.SubmitChanges();
            return true;
        }
        public bool Delete(int id)
        {
            model.ObjectTrackingEnabled = true;
            var query = model.KeyValues.FirstOrDefault(kv => kv.Id == id);
            model.KeyValues.DeleteOnSubmit(query);
            model.SubmitChanges();
            return true;
        }
    }
}
