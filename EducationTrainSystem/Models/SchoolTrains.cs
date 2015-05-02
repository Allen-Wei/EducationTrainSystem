using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;

namespace EducationTrainSystem.Models
{
    public partial class SchoolTrain
    {
        public List<SchoolSubject> SchoolSubjects { get; set; }
        partial void OnCreated()
        {
            throw new NotImplementedException();
        }
    }
}