using System.Web.Mvc;

namespace Education.Areas.APIv2
{
    public class APIv2AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "APIv2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "APIv2_default",
                "APIv2/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
