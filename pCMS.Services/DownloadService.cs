using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using pCMS.Core;
using pCMS.Data;

namespace pCMS.Services
{
    public  interface IDownloadService
    {
        FileDownload GetDownloadById(Guid downloadId);

        void DeleteDownload(FileDownload download);
        void DeleteDownload(Guid downloadId);

        void InsertDownload(FileDownload download, byte[] binary);

        void UpdateDownload(FileDownload download);

        byte[] GetFileBinary(Guid downloadId);
        string GetFilePath(Guid downloadId);
    }

    public class DownloadService : IDownloadService
    {
        #region Fields
        private readonly IDalContext _context;
        private readonly IWebHelper _webHelper;
        private static readonly object SLock = new object();

        public DownloadService(IDalContext context, IWebHelper webHelper)
        {
            _context = context;
            _webHelper = webHelper;
        }

        #endregion

        #region Ctor

        
        #endregion
        public FileDownload GetDownloadById(Guid downloadId)
        {
            return _context.FileDownloads.Find(q => q.Id == downloadId);
        }

        public void DeleteDownload(FileDownload download)
        {
            if (download == null)
                throw new ArgumentNullException("download");

            _context.FileDownloads.Delete(download);
        }

        public void DeleteDownload(Guid downloadId)
        {
            var download = GetDownloadById(downloadId);
            if (download != null)
            {
                File.Delete(GetFilePath(downloadId));
                DeleteDownload(GetDownloadById(downloadId));
               
            }
        }

        public void InsertDownload(FileDownload download,byte[] binary)
        {
            if (download == null)
                throw new ArgumentNullException("download");
            SaveFile(download.Id, download.Filename, download.Extension, binary);
            _context.FileDownloads.Create(download);

        }

        public void UpdateDownload(FileDownload download)
        {
            if (download == null)
                throw new ArgumentNullException("download");

            _context.FileDownloads.Update(download);    
        }

        public byte[] GetFileBinary(Guid downloadId)
        {
            var download = GetDownloadById(downloadId);
            return File.ReadAllBytes(Path.Combine(_webHelper.MapPath("~/content/filedownload/"), string.Format("{0}_{1}{2}", downloadId.ToString("N").ToLower(), download.Filename, download.Extension)));
        }

        public string GetFilePath(Guid downloadId)
        {
            var download = GetDownloadById(downloadId);
            return Path.Combine(_webHelper.MapPath("~/content/filedownload/"),
                string.Format("{0}_{1}{2}", downloadId.ToString("N").ToLower(), download.Filename, download.Extension));
        }

        protected virtual void SaveFile(Guid downloadId,string filename,string extention, byte[] binary)
        {
            var fileName = string.Format("{0}_{1}{2}", downloadId.ToString("N").ToLower(), filename, extention);
            File.WriteAllBytes(GetLocalPath(fileName), binary);
        }

        protected virtual string GetLocalPath(string fileName, string imagesDirectoryPath = null)
        {
            if (String.IsNullOrEmpty(imagesDirectoryPath))
            {
                imagesDirectoryPath = _webHelper.MapPath("~/content/filedownload/");
            }
            var filePath = Path.Combine(imagesDirectoryPath, fileName);
            return filePath;
        }
    }
}
