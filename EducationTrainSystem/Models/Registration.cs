using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationTrainSystem.Models
{
    public partial class Registration
    {
        public enum TrainsCategory
        {
            EduTrains,
            CertificationTrains,
            SchoolTrains
        }

        public void Initial()
        {
            this.Gid = Guid.NewGuid();
            this.GenerateDate = DateTime.Now;
        }
    }
}