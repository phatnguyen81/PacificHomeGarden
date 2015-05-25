using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using pCMS.Framework;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class PictureController : BaseAdminController
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> attachments)
        {
                foreach (var file in attachments)
                {
                    file.GetPictureBits();
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (!String.IsNullOrEmpty(fileExtension))
                        fileExtension = fileExtension.ToLowerInvariant();
                    switch (fileExtension)
                    {
                        case ".bmp":
                            break;
                        case ".gif":
                            break;
                        case ".jpeg":
                        case ".jpg":
                        case ".jpe":
                        case ".jfif":
                        case ".pjpeg":
                        case ".pjp":
                            break;
                        case ".png":
                            break;
                        case ".tiff":
                        case ".tif":
                            break;
                        default:
                            break;
                    }


                    //var picture = pictureRepository.Add(pictureBinary, contentType, true);


                    var fileName = Path.GetFileName(file.FileName);

                    // The files are not actually saved in this demo
                    // file.SaveAs(physicalPath);
                }
                // Return an empty string to signify success
                return Content("");
        }
        [HttpPost]
        public ActionResult AsyncUpload()
        {
          
            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    default:
                        break;
                }
            }

            var picture = _pictureService.InsertPicture(fileBinary, contentType, null, true);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetPictureUrl(picture, 100)
            },
                "text/plain");
        }
        //public ActionResult InsertPicture(string authToken, HttpPostedFileBase httpPostedFile)
        //{
        //    using (var entities = new pCMSEntities())
        //    {
        //        var pictureService = new PictureService(entities);
        //        var pictureBinary = httpPostedFile.GetPictureBits();

        //        var contentType = httpPostedFile.ContentType;
        //        var fileExtension = Path.GetExtension(httpPostedFile.FileName);
        //        if (!String.IsNullOrEmpty(fileExtension))
        //            fileExtension = fileExtension.ToLowerInvariant();
        //        switch (fileExtension)
        //        {
        //            case ".bmp":
        //                contentType = "image/bmp";
        //                break;
        //            case ".gif":
        //                contentType = "image/gif";
        //                break;
        //            case ".jpeg":
        //            case ".jpg":
        //            case ".jpe":
        //            case ".jfif":
        //            case ".pjpeg":
        //            case ".pjp":
        //                contentType = "image/jpeg";
        //                break;
        //            case ".png":
        //                contentType = "image/png";
        //                break;
        //            case ".tiff":
        //            case ".tif":
        //                contentType = "image/tiff";
        //                break;
        //            default:
        //                break;
        //        }

        //        var picture = pictureRepository.Add(pictureBinary, contentType, true);
        //        return Json(new { pictureId = picture.Id, imageUrl = pictureRepository.GetPictureUrl(picture, 100) });
        //    }
        //}

        //public ActionResult AsyncUpload(string authToken)
        //{
        //    return InsertPicture(authToken, Request.Files[0]);
        //}

        public ActionResult View(Guid id)
        {
            return View();
        }
    }
}