using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using pCMS.Core;
using pCMS.Core.Domain;
using pCMS.Framework;
using pCMS.Framework.Helpers;
using pCMS.Models;
using pCMS.Services;
using pCMS.Utils;
namespace pCMS.Controllers
{
    [Authorize]
    public class FactoryDirectController : BaseController
    {
        private readonly IWebHelper _webHelper;
        private readonly ICollectionService _collectionService;
        private readonly IPictureService _pictureService;

        public FactoryDirectController(IWebHelper webHelper, ICollectionService collectionService, IPictureService pictureService)
        {
            _webHelper = webHelper;
            _collectionService = collectionService;
            _pictureService = pictureService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CollectionList()
        {
            var model = new CollectionListModel
            {
                Collections = _collectionService.Search(null).Select(q => new CollectionModel
                {
                    Id = q.Id,
                    Alias = q.Alias,
                    Title = q.Title,
                    ShortDesciption = CommonHelper.StripHTML(q.ShortDescription),
                    PictureUrl =
                        _pictureService.GetPictureUrl(q.PictureId, 300)
                }).ToList()
            };
            return PartialView(model);
        }

        public ActionResult CollectionDetail(string id)
        {
            var  model = new CollectionModel();
            var collection = _collectionService.GetByAlias(id);
            if (collection == null) return RedirectToAction("Index");
            model.Id = collection.Id;
            model.Title = collection.Title;
            model.ShortDesciption = collection.ShortDescription;
            model.FullDescription = collection.FullDescription;
            model.PictureUrl = _pictureService.GetPictureUrl(collection.PictureId);
            model.FileDownloadId = collection.FileDownloadId;
            return View(model);
        }

        public ActionResult ArroundCollections(Guid id)
        {
            var model = new CollectionListModel
            {
                Collections = _collectionService.Arround(id).Select(q => new CollectionModel
                {
                    Id = q.Id,
                    Alias = q.Alias,
                    Title = q.Title,
                    ShortDesciption = CommonHelper.StripHTML(q.ShortDescription),
                    PictureUrl =
                        _pictureService.GetPictureUrl(q.PictureId, 300)
                }).ToList()
            };
            return PartialView(model);
        }
    }
}
