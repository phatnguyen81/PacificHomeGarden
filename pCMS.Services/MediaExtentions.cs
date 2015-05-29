using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace pCMS.Services
{
    public static class MediaExtentions
    {
        public static byte[] GetDownloadBits(this HttpPostedFileBase postedFile)
        {
            Stream fs = postedFile.InputStream;
            int size = postedFile.ContentLength;
            var binary = new byte[size];
            fs.Read(binary, 0, size);
            return binary;
        }
    }
}
