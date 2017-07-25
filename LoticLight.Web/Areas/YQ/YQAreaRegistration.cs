using System.Web.Mvc;

namespace LoticLight.Web.Areas.YQ
{
    public class YQAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "YQ";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "YQ_default",
                "YQ/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}