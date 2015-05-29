using System;
using System.Configuration;
using System.Web;

namespace pCMS.Core
{
    public static class AppSettings
    {
        public static string DefaultImageName
        {
            get { return ConfigurationManager.AppSettings["DefaultImageName"]; }
        }
        public static string DefaultAvatarImageName
        {
            get { return ConfigurationManager.AppSettings["DefaultAvatarImageName"]; }
        }
        public static string RelatePicturePath
        {
            get { return ConfigurationManager.AppSettings["PicturePath"]; }
        }

        public static string PicturePath
        {
            get { return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PicturePath"]); }
        }
        public static string DefaultLanguage
        {
            get
            {

                return ConfigurationManager.AppSettings["DefaultLanguage"] ?? "en";
            }
        }
        public static bool StoreInDb
        {
            get { return ConfigurationManager.AppSettings["StoreInDb"].ToLower() == "true"; }
        }
        public static int MaximumImageSize
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["MaximumImageSize"]);
                }
                catch
                {
                    return 1048576;
                }
                
            }
        }

        public static Guid CustommersAlbum
        {
            get
            {
                try
                {
                    return new Guid(ConfigurationManager.AppSettings["CustommersAlbum"]);
                }
                catch
                {
                    return Guid.Empty;
                }
            }
        }
        public static Guid ProductsInGardensAlbum
        {
            get
            {
                try
                {
                    return new Guid(ConfigurationManager.AppSettings["ProductsInGardensAlbum"]);
                }
                catch
                {
                    return Guid.Empty;
                }
            }
        }
        public static Guid ProductsInStoresAlbum
        {
            get
            {
                try
                {
                    return new Guid(ConfigurationManager.AppSettings["ProductsInStoresAlbum"]);
                }
                catch
                {
                    return Guid.Empty;
                }
            }
        }

        public static Guid SliderOnFactoryDirect
        {
            get
            {
                try
                {
                    return new Guid(ConfigurationManager.AppSettings["SliderOnFactoryDirect"]);
                }
                catch
                {
                    return Guid.Empty;
                }
            }
        }
        public static string EmailReceive
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["EmailReceive"];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}