using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EducationTrainSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EducationTrainSystem.APIv1
{
    public class CollegeController : ApiController
    {
        EducationTrain model = new EducationTrain();
        public IEnumerable<KeyValue> Get()
        {
            model.ObjectTrackingEnabled = false;
            return model.KeyValues.Where(kv => kv.Mark == "college");
        }
        
    }
}
