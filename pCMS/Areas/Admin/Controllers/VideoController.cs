using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core;
using pCMS.Core.Utils;
using pCMS.Framework.Controllers;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class VideoController : BaseAdminController
    {
        private readonly IVideoService _videoService;
        private readonly IPictureService _pictureService;

        public VideoController(IVideoService videoService, IPictureService pictureService)
        {
            _videoService = videoService;
            _pictureService = pictureService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Videos(GridCommand command)
        {
            var videos = _videoService.GetAll().OrderBy(q=>q.DisplayOrder);
            var model = new GridModel<VideoModel>
            {
                Data = videos.Select(
                    q => new VideoModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        VideoUrl = q.VideoUrl,
                        DisplayOrder = q.DisplayOrder
                    }).ToList()
            };

            return new JsonResult
            {
                Data = model
            };
        }
        public ActionResult List(VideoListModel model)
        {
            var videos = _videoService.GetAll().OrderBy(q => q.DisplayOrder);
            model.Videos = new GridModel<VideoModel>
            {
                Data = videos.Select(
                    q => new VideoModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        VideoUrl = q.VideoUrl,
                        DisplayOrder = q.DisplayOrder
                    }).ToList()
            };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new VideoModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(VideoModel model, bool continueEditing)
        {
            try
            {


                var video = new Video
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    PictureId = model.PictureId,
                    VideoUrl = model.VideoUrl,
                    DisplayOrder = model.DisplayOrder
                };
                _videoService.Add(video);
                SuccessNotification("Add video '" + model.Title + "' successful");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = video.Id })
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
            var video = _videoService.GetById(id);
            if (video == null) return RedirectToAction("List");

            var model = new VideoModel()
            {
                Id = video.Id,
                Title = video.Title,
                PictureId = video.PictureId,
                VideoUrl = video.VideoUrl,
                DisplayOrder = video.DisplayOrder
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(VideoModel model, bool continueEditing)
        {
            try
            {
              
                var video = _videoService.GetById(model.Id);
                if (video == null) return RedirectToAction("List");
                var prevPictureId = video.PictureId;
                video.Title = model.Title;
                video.VideoUrl = model.VideoUrl;

                video.DisplayOrder = model.DisplayOrder;

                video.PictureId = model.PictureId;



                //delete an old picture (if deleted or updated)
                if (prevPictureId != Guid.Empty && prevPictureId != video.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }

                _videoService.SaveChanges();

                SuccessNotification("Update video '" + model.Title + "' successful");
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
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
                var video = _videoService.GetById(id);
                if (video.PictureId != Guid.Empty) _pictureService.DeletePicture(video.PictureId);
                _videoService.Delete(id);

                SuccessNotification("Delete video '" + video.Title + "' successful");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}