using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationTrainSystem.ViewModels
{
    public class QueryParams
    {
        public string Condition { get; set; }
        public object[] Parameters { get; set; }
        public string Order { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}