using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core;
using pCMS.Core.Utils;
using pCMS.Framework;
using pCMS.Framework.Controllers;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class AlbumController : BaseAdminController
    {
        private readonly IAlbumService _albumService;
        private readonly IPictureService _pictureService;
        public AlbumController(IAlbumService albumService,IPictureService pictureService )
        {
            _albumService = albumService;
            _pictureService = pictureService;
        }

        #region Ajax Methods
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AlbumPictures(GridCommand command, Guid albumId)
        {
            
            var albumPictures = _albumService.GetAllPictures(albumId);
            if(albumPictures == null)
            {
                return Content("Pictures null");
            }
            var countRecord = albumPictures.Count();
            var albumPictureListModel = albumPictures.OrderBy(q => q.DisplayOrder).Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                .Select(x => new AlbumItemModel.PictureListModel()
                {
                    PictureId = x.Picture.Id,
                    AlbumId = x.AlbumId,
                    MineType = x.Picture.MimeType,
                    Description = x.Description,
                    DisplayOrder = x.DisplayOrder,
                    PictureUrl = _pictureService.GetPictureUrl(x.Picture)
                })
                .ToList();

            var model = new GridModel<AlbumItemModel.PictureListModel>
            {
                Data = albumPictureListModel,
                Total = countRecord
            };

            return new JsonResult
            {
                Data = model
            };
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AlbumPictureDelete(GridCommand command, AlbumItemModel.PictureListModel model)
        {
            var album = _albumService.GetById(model.AlbumId);
            var albumPicture = album.Album_Picture.FirstOrDefault(q => q.PictureId == model.PictureId);
            if (albumPicture == null) throw new ArgumentException("No picture found with the specified id", "id");
            _albumService.DeleteAlbumPicture(albumPicture);
            return AlbumPictures(command, model.AlbumId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AlbumPictureUpdate(GridCommand command, AlbumItemModel.PictureListModel model)
        {
            var album = _albumService.GetById(model.AlbumId);
            var albumPicture = album.Album_Picture.FirstOrDefault(q => q.PictureId == model.PictureId);
            if (albumPicture == null) throw new ArgumentException("No picture found with the specified id", "id");
            albumPicture.Description = model.Description;
            albumPicture.DisplayOrder = model.DisplayOrder;
            _albumService.Update(album);
            return AlbumPictures(command, model.AlbumId);
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Albums(GridCommand command)
        {
            var albums = _albumService.SearchAlbums(null, null, true, command.Page - 1, command.PageSize);
            var model = new GridModel<AlbumModel>
                            {
                                Data = albums.Select(
                                    q => new AlbumModel()
                                             {
                                                 Id = q.Id,
                                                 Title = q.Title,
                                                 Description = q.Description,
                                                 Alias = q.Alias,
                                                 IsPublished = q.IsPublished
                                             }).ToList(),
                                Total = albums.TotalCount
                            };

            return new JsonResult
            {
                Data = model
            };
        }
        #endregion

        #region Action
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(AlbumListModel model)
        {
            var albums = _albumService.SearchAlbums(null, null, true, 0, 20);
            model.Albums = new GridModel<AlbumModel>
                               {
                                   Data = albums.Select(
                                       q => new AlbumModel
                                                {
                                                    Id = q.Id,
                                                    Title = q.Title,
                                                    Description = q.Description,
                                                    Alias = q.Alias,
                                                    IsPublished = q.IsPublished
                                                }).ToList(),
                                   Total = albums.TotalCount
                               };
            return View(model);
        }

        public ActionResult Create()
        {

            var model = new AlbumItemModel();
            return View(model);
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(AlbumItemModel model, bool continueEditing)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _albumService.CheckExistAlias(model.Alias))
                {
                    throw new Exception("Alias này đã tồn tại");
                }
                if (string.IsNullOrWhiteSpace(model.Alias))
                {
                    var i = 0;
                    if (string.IsNullOrWhiteSpace(model.Alias))
                    {
                        do
                        {
                            model.Alias = (i == 0
                                               ? StringHelpers.MakeSEOTitle(model.Title)
                                               : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                            i++;
                        } while (_albumService.CheckExistAlias(model.Alias));
                    }
                }

                var album = new Album
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Alias = model.Alias,
                    Description = model.Description,
                    IsPublished = model.IsPublished
                };
                _albumService.Add(album);
                SuccessNotification("Thêm mới album '" + model.Title + "' thành công");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = album.Id })
                            : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }

            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            var album = _albumService.GetById(id);
            var model = new AlbumItemModel
            {
                Alias = album.Alias,
                Title = album.Title,
                Id = album.Id,
                Description = album.Description,
                IsPublished = album.IsPublished
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(AlbumItemModel model, bool continueEditing)
        {
            try
            {


                if (!string.IsNullOrWhiteSpace(model.Alias) && _albumService.CheckExistAlias(model.Alias, model.Id))
                {
                    throw new Exception("Alias này đã tồn tại");
                }
                var i = 0;
                if (string.IsNullOrWhiteSpace(model.Alias))
                {
                    do
                    {
                        model.Alias = (i == 0
                                            ? StringHelpers.MakeSEOTitle(model.Title)
                                            : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                        i++;
                    } while (_albumService.CheckExistAlias(model.Alias, model.Id));
                }
                var album = _albumService.GetById(model.Id);
                if (album == null) return RedirectToAction("List");

                album.Title = model.Title;
                album.Alias = model.Alias;
                album.Description = model.Description;
                album.IsPublished = model.IsPublished;
                _albumService.Update(album);
                SuccessNotification("Update album '" + model.Title + "' successful");
                return continueEditing
                                ? RedirectToAction("Edit", new { id = album.Id })
                                : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var album = _albumService.GetById(id);

                _albumService.Delete(id);

                SuccessNotification("Delete album '" + album.Title + "' successful");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("Edit", new { id });
        }

        public ActionResult UploadPictures(IEnumerable<HttpPostedFileBase> attachments, Guid albumId)
        {
            var album = _albumService.GetById(albumId);
            var maxDisplayOrder = album.Album_Picture.Count == 0
                                            ? 0
                                            : album.Album_Picture.Max(q => q.DisplayOrder);
            foreach (var file in attachments)
            {
                var picture = _pictureService.InsertPicture(file.GetPictureBits(), file.ContentType, null,
                                                            false);
                var albumPicture = new Album_Picture
                                        {
                                            AlbumId = album.Id,
                                            PictureId = picture.Id,
                                            DisplayOrder = ++maxDisplayOrder
                                        };

                album.Album_Picture.Add(albumPicture);
                    
            }
            _albumService.Update(album);
            return Content("");
        }
        #endregion
    }
}
