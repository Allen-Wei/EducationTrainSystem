using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using EducationTrainSystem.Models;
using EducationTrainSystem.Library;

namespace EducationTrainSystem.APIv1
{
    [Authorize(Roles="sales")]
    public class RegEntityController : ApiController
    {
       
        private EducationTrain model = new EducationTrain();

        private RegEntityController()
        {
            model.ObjectTrackingEnabled = false;
        }

        public RegEntity Get(int id)
        {
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
                         where reg.Id == id
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         }).Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         })
                         .FirstOrDefault();

          
            return query;
        }

        [AllowAnonymous]
        public RegEntity GetDetail(Guid gid)
        {
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
                         where reg.Gid == gid
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         }).Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         })
                        .FirstOrDefault();

            return query;
        }


        public IEnumerable<RegEntity> Get(int take, int skip)
        {
            var total = model.Registrations.LongCount();
            var pages = Math.Ceiling((Convert.ToDouble(total) / Convert.ToDouble(take)));
            HttpContext.Current.Response.AddHeader("X-HeyHey-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-HeyHey-Pages", pages.ToString());
            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
                         orderby reg.Id descending
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         })
                         .Skip(skip)
                         .Take(take)
                         .ToList()
                         .Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         });


            return query;
        }

        public IEnumerable<RegEntity> Get(string train, int take, int skip)
        {
            var total = model.Registrations.Where(r => r.TrainCategory == train).LongCount();
            var pages = Math.Ceiling((Convert.ToDouble(total) / Convert.ToDouble(take)));
            HttpContext.Current.Response.AddHeader("X-HeyHey-Total", total.ToString());
            HttpContext.Current.Response.AddHeader("X-HeyHey-Pages", pages.ToString());

            var query = (from reg in model.Registrations
                         join user in model.RegUsers.DefaultIfEmpty() on reg.RegUserId equals user.Gid into ru
                         join edu in model.EduTrains.DefaultIfEmpty() on reg.TrainId equals edu.Gid into et
                         join cert in model.CertificationTrains.DefaultIfEmpty() on reg.TrainId equals cert.Gid into ct
                         join school in model.SchoolTrains.DefaultIfEmpty() on reg.TrainId equals school.Gid into st
                         where reg.TrainCategory == train
                         orderby reg.Id descending
                         select new
                         {
                             reg,
                             ru,
                             et,
                             ct,
                             st
                         })
                         .Skip(skip)
                         .Take(take)
                         .ToList()
                         .Select(obj => new RegEntity()
                         {
                             Reg = obj.reg,
                             User = obj.ru.FirstOrDefault(),
                             Edu = obj.et.FirstOrDefault(),
                             Cert = obj.ct.FirstOrDefault(),
                             School = obj.st.FirstOrDefault()
                         });


            return query;
        }

        public bool Put(RegEntity entity)
        {
            return entity.Add();
        }

        //update 
        public bool Post(RegEntity entity)
        {

            var regEntity = new RegEntity();
            return regEntity.Update(entity); 
        }

        public bool Delete(int id)
        {
            var regEntity = new RegEntity();
            return regEntity.Delete(id); 
        }


        [Route("apiv1/RegEntity/Apply")]
        [HttpPost]
        public bool Apply() {
            return true;
        }
    }
}
