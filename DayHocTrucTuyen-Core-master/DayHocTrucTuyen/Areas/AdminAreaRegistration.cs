using System.Web.Mvc;

namespace DayHocTrucTuyen.Areas
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Blog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Blog_default",
                "Blog/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }
    }
}
