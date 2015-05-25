using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace pCMS.Framework.Helpers
{
    public static class FileHelper
    {
        public static string GetTemplateFileContent(string filename)
        {
            var filepath = HttpContext.Current.Server.MapPath("~/Templates/" + filename);
            if (!File.Exists(filepath)) return "";
            var myFile = new StreamReader(filepath);
            return myFile.ReadToEnd();
        }
    }
}
