using System;
using System.Web.Mvc;
using pCMS.Admin.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Controllers
{
    public class DownloadController : BaseController
    {
        private readonly IDownloadService _downloadService;

        public DownloadController(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        public FileResult GetFileUpload(Guid downloadId)
        {

            var download = _downloadService.GetDownloadById(downloadId);
            //if (download == null)
            //    return Content("Download is not available any more.");

            //return result
            string fileName = download.Filename + download.Extension;
            string contentType = !String.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + CommonHelper.MakeValidFileName(fileName) + ";");
            return File(_downloadService.GetFilePath(downloadId), contentType);
            //return new File(_downloadService.GetFileBinary(download.Id), contentType) { FileDownloadName = fileName + download.Extension };
        }
    }
}