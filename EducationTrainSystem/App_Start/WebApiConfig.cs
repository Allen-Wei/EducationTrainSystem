using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EducationTrainSystem
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Arrays;
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "apiv1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
