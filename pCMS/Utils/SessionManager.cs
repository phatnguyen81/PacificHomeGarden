using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pCMS.Core;
using pCMS.Order;

namespace pCMS.Utils
{
    public static class SessionManager
    {
        public static ShoppingCart CurrentShoppingCard
        {
            get
            {
                //if (HttpContext.Current.Session["CurrentShoppingCard"] == null)
                //    HttpContext.Current.Session["CurrentShoppingCard"] = new ShoppingCart();
                //return (ShoppingCart) HttpContext.Current.Session["CurrentShoppingCard"];
                return new ShoppingCart();
            }
            //set { HttpContext.Current.Session["CurrentShoppingCard"] = value; }
        }

        public static bool FirstVisit
        {
            get
            {
                if (HttpContext.Current.Session["FirstVisit"] == null)
                    HttpContext.Current.Session["FirstVisit"] = true;
                return (bool)HttpContext.Current.Session["FirstVisit"];
            }
            set { HttpContext.Current.Session["FirstVisit"] = value; }
        }

        public static string ContinueShoppingUrl
        {
            get
            {
                if (HttpContext.Current.Session["ContinueShoppingUrl"] == null)
                    HttpContext.Current.Session["ContinueShoppingUrl"] = "";
                return HttpContext.Current.Session["ContinueShoppingUrl"].ToString();
            }
            set { HttpContext.Current.Session["ContinueShoppingUrl"] = value; }
        }
    }
}