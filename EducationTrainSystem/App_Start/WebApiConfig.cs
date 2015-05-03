using System.Web.Http;

namespace EducationTrainSystem
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "apiv1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
