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


        public CollectionController(ILocalizationService localizationService, ICollectionService collectionService, IPictureService pictureService)
        {
            _localizationService = localizationService;
            _collectionService = collectionService;
            _pictureService = pictureService;
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

                var collection = new Collection
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Alias = model.Alias,
                    ShortDescription = model.ShortDescription,
                    FullDescription = model.FullDescription,
                    PictureId = model.PictureId,
                    FileDownloadId = model.FileDownloadId
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
                ErrorNotification(ex.GetBaseException().Message, false);
            }

            return View(model);
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
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(CollectionModel model, bool continueEditing)
        {
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
                collection.FileDownloadId = model.FileDownloadId;


                collection.PictureId = model.PictureId;

                _collectionService.SaveChanges();

                //delete an old picture (if deleted or updated)
                if (prevPictureId !=Guid.Empty && prevPictureId != collection.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
              

                SuccessNotification("Update collection '" + model.Title + "' successful !!!");
                return continueEditing
                                ? RedirectToAction("Edit", new { id = collection.Id })
                                : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }

            return View(model);

        }

        #endregion
    }
}
