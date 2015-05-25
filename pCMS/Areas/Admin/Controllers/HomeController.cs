using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pCMS.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public ActionResult AdminMenu()
        {
            return PartialView();
        }
        public ActionResult MessageBox()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            return View();
        }


       
    }
}
