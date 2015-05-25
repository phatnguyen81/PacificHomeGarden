using System.Web.Mvc;

namespace pCMS.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", area = "Admin", id = ""},
                new[] {"pCMS.Admin.Controllers"}
                );
            context.MapRoute(
                "NetAdvImage",
                "Content/editors/tinymce/plugins/netadvimage/{action}",
                new {controller = "NetAdvImage"}
                );
        }
    }
}
