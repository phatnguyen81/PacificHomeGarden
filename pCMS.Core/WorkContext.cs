using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Security;
using pCMS.Core.Domain;

namespace pCMS.Core
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string TimeZoneId { get; set; }
    }

    public static class WorkContext
    {
        private static UserInfo _userLoginInfo;
        public static UserInfo UserLoginInfo
        {
            get
            {
                if(_userLoginInfo == null)
                {
                    var user = Membership.GetUser(HttpContext.Current.User.Identity.Name);
                    if (user == null) return null;
                    var profile = AccountProfile.GetUserProfile(HttpContext.Current.User.Identity.Name);
                    _userLoginInfo = new UserInfo
                           {
                               UserName= user.UserName,
                               TimeZoneId = profile.TimeZoneId
                           };
                }

                return _userLoginInfo;
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                return HttpContext.Current.Request.Cookies["lang"] == null
                           ? "en"
                           : HttpContext.Current.Request.Cookies["lang"].Value;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(value);
                HttpContext.Current.Response.SetCookie(new HttpCookie("lang", value));
            }
        }
    }
}