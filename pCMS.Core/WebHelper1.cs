using System;
using System.Web;

//namespace pCMS.Core
//{
//    public class WebHelper1
//    {
//        private readonly HttpContextBase _httpContext;

//        /// <summary>
//        /// Ctor
//        /// </summary>
//        /// <param name="httpContext">HTTP context</param>
//        public WebHelper1(HttpContextBase httpContext)
//        {
//            this._httpContext = httpContext;
//        }

//        public static string GetHostUrl(string webpath)
//        {
//            var url = VirtualPathUtility.ToAbsolute(webpath);
//            if (!url.EndsWith("/"))
//            {
//                url += "/";
//            }
//            return url;
//        }

//        public virtual string GetUrlReferrer()
//        {
//            string referrerUrl = string.Empty;

//            if (_httpContext != null &&
//                _httpContext.Request != null &&
//                _httpContext.Request.UrlReferrer != null)
//                referrerUrl = _httpContext.Request.UrlReferrer.ToString();

//            return referrerUrl;
//        }

//        /// <summary>
//        /// Get context IP address
//        /// </summary>
//        /// <returns>URL referrer</returns>
//        public virtual string GetCurrentIpAddress()
//        {
//            if (_httpContext != null &&
//                    _httpContext.Request != null &&
//                    _httpContext.Request.UserHostAddress != null)
//                return _httpContext.Request.UserHostAddress;
//            else
//                return string.Empty;
//        }

//        /// <summary>
//        /// Gets this page name
//        /// </summary>
//        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
//        /// <returns>Page name</returns>
//        public virtual string GetThisPageUrl(bool includeQueryString)
//        {
//            bool useSsl = IsCurrentConnectionSecured();
//            return GetThisPageUrl(includeQueryString, useSsl);
//        }

//        /// <summary>
//        /// Gets this page name
//        /// </summary>
//        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
//        /// <param name="useSsl">Value indicating whether to get SSL protected page</param>
//        /// <returns>Page name</returns>
//        public virtual string GetThisPageUrl(bool includeQueryString, bool useSsl)
//        {
//            string url = string.Empty;
//            if (_httpContext == null)
//                return url;

//            if (includeQueryString)
//            {
//                string storeHost = GetStoreHost(useSsl);
//                if (storeHost.EndsWith("/"))
//                    storeHost = storeHost.Substring(0, storeHost.Length - 1);
//                url = storeHost + _httpContext.Request.RawUrl;
//            }
//            else
//            {
//                url = _httpContext.Request.Url.GetLeftPart(UriPartial.Path);
//            }
//            url = url.ToLowerInvariant();
//            return url;
//        }
//    }
//}