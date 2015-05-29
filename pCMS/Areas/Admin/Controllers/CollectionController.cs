using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core;
using pCMS.Core.Domain;
using pCMS.Core.Utils;
using pCMS.Framework.Controllers;
using pCMS.Framework.Helpers;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class CollectionController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly ICollectionService _collectionService;
        private readonly IPictureService _pictureService;
        private readonly IDownloadService _downloadService;


        public CollectionController(ILocalizationService localizationService, ICollectionService collectionService, IPictureService pictureService, IDownloadService downloadService)
        {
            _localizationService = localizationService;
            _collectionService = collectionService;
            _pictureService = pictureService;
            _downloadService = downloadService;
        }


        #region Actions
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Collections(GridCommand command)
        {
            var Collections = _collectionService.Search(null, command.Page - 1, command.PageSize);
            var model = new GridModel<CollectionModel>
            {
                Data = Collections.Select(
                    q => new CollectionModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias,
                    }).ToList(),
                Total = Collections.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }
        public ActionResult List(CollectionListModel model)
        {
            var collections = _collectionService.Search(null, 0, 20);
            model.Collections = new GridModel<CollectionModel>
            {
                Data = collections.Select(
                    q => new CollectionModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias
                    }).ToList(),
                Total = collections.TotalCount
            };
            return View(model);
        }
        public ActionResult Create()
        {
            var model = new CollectionModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(CollectionModel model, bool continueEditing)
        {
            Guid downloadId = Guid.Empty;
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _collectionService.CheckExistAlias(model.Alias))
                {
                    throw new Exception("Alias này đã tồn tại");
                }
                if (string.IsNullOrWhiteSpace(model.Alias))
                {
                    var i = 0;
                    do
                    {
                        model.Alias = (i == 0
                                           ? StringHelpers.MakeSEOTitle(model.Title)
                                           : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                        i++;
                    } while (_collectionService.CheckExistAlias(model.Alias));
                }
                var httpPostedFile = this.Request.Files["FileDownload"];
                if ((httpPostedFile != null) && (!String.IsNullOrEmpty(httpPostedFile.FileName)))
                {
                    downloadId = Guid.NewGuid();
                    var download = new FileDownload
                    {
                        Id = downloadId,
                        ContentType = httpPostedFile.ContentType,
                        Filename = Path.GetFileNameWithoutExtension(httpPostedFile.FileName),
                        Extension = Path.GetExtension(httpPostedFile.FileName)
                    };
                    _downloadService.InsertDownload(download, httpPostedFile.GetDownloadBits());
                }
                var collection = new Collection
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Alias = model.Alias,
                    ShortDescription = model.ShortDescription,
                    FullDescription = model.FullDescription,
                    PictureId = model.PictureId,
                    FileDownloadId = downloadId
                };
                _collectionService.Add(collection);
                _collectionService.SaveChanges();
                SuccessNotification("Create collection '" + model.Title + "' successful");

                return continueEditing
                            ? RedirectToAction("Edit", new { id = collection.Id })
                            : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _downloadService.DeleteDownload(downloadId);
                ErrorNotification(ex.GetBaseException().Message, false);
            }

            return View(model);
        }

        protected void PrepareCollectionModel(CollectionModel model)
        {
            if (model.FileDownloadId != Guid.Empty)
            {
                var download = _downloadService.GetDownloadById(model.FileDownloadId);
                if (download != null)
                {
                    model.FileName = download.Filename + download.Extension;
                }
            }
            model.DeleteFile = false;
        }
        public ActionResult Edit(Guid id, string selectedTab)
        {
            ViewBag.SelectedTab = Request["selectedTab"];
            var collection = _collectionService.GetById(id);
            var model = new CollectionModel
            {
                Alias = collection.Alias,
                Title = collection.Title,
                Id = collection.Id,
                ShortDescription = collection.ShortDescription,
                FullDescription = collection.FullDescription,
                PictureId = collection.PictureId,
                FileDownloadId = collection.FileDownloadId
            };
            PrepareCollectionModel(model);
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(CollectionModel model, bool continueEditing)
        {
            var downloadId = Guid.Empty;
            ;
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Alias) && _collectionService.CheckExistAlias(model.Alias, model.Id))
                {
                    throw new Exception("Alias này đã tồn tại");
                }
                if (string.IsNullOrWhiteSpace(model.Alias))
                {
                    var i = 0;
                    do
                    {
                        model.Alias = (i == 0
                                           ? StringHelpers.MakeSEOTitle(model.Title)
                                           : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                        i++;
                    } while (_collectionService.CheckExistAlias(model.Alias, model.Id));
                }
                var collection = _collectionService.GetById(model.Id);
                if (collection == null) return RedirectToAction("List");

                var prevPictureId = collection.PictureId;

                collection.Title = model.Title;
                collection.Alias = model.Alias;
                collection.ShortDescription = model.ShortDescription;
                collection.FullDescription = model.FullDescription;
                var oldDownloadId = model.FileDownloadId;
                //collection.FileDownloadId = model.FileDownloadId;
                


                collection.PictureId = model.PictureId;

              

                //delete an old picture (if deleted or updated)
                if (prevPictureId !=Guid.Empty && prevPictureId != collection.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }

                if (model.DeleteFile)
                {
                    _downloadService.DeleteDownload(model.FileDownloadId);
                    oldDownloadId = Guid.Empty;
                }
                else
                {
                    var httpPostedFile = this.Request.Files["FileDownload"];
                    if ((httpPostedFile != null) && (!String.IsNullOrEmpty(httpPostedFile.FileName)))
                    {
                        downloadId = Guid.NewGuid();
                        var download = new FileDownload
                        {
                            Id = downloadId,
                            ContentType = httpPostedFile.ContentType,
                            Filename = Path.GetFileNameWithoutExtension(httpPostedFile.FileName),
                            Extension = Path.GetExtension(httpPostedFile.FileName)
                        };
                        _downloadService.InsertDownload(download, httpPostedFile.GetDownloadBits());
                    }
                    if (oldDownloadId != Guid.Empty)
                    {
                        _downloadService.DeleteDownload(oldDownloadId);
                    }
                    oldDownloadId = downloadId;
                }
                collection.FileDownloadId = oldDownloadId;

                _collectionService.SaveChanges();

                SuccessNotification("Update collection '" + model.Title + "' successful !!!");
                return continueEditing
                                ? RedirectToAction("Edit", new { id = collection.Id })
                                : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _downloadService.DeleteDownload(downloadId);
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            PrepareCollectionModel(model);
            return View(model);

        }

        public ActionResult Up(Guid id)
        {
            try
            {
                _collectionService.Up(id);
                _collectionService.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
                throw;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Down(Guid id)
        {
            try
            {
                _collectionService.Down(id);
                _collectionService.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
                throw;
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
