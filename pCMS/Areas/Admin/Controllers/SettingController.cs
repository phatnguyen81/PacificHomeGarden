using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult SettingList(GridCommand command)
        {
            var settings =
                    _settingService.GetAll().Select(
                        q => new SettingListModel()
                        {
                            Id = q.Id,
                            Key = q.Key,
                            Value = q.Value,
                            LanguageCode = q.LanguageCode,
                            Description = q.Description
                        }).ToList();

            var model = new GridModel<SettingListModel>
            {
                Data = settings,
                Total = settings.Count
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult InsertSetting(GridCommand command, SettingListModel model)
        {

            if (_settingService.GetAll().Any(q => q.Key == model.Key && q.LanguageCode == model.LanguageCode))
            {
                return Content("Setting existed");
            }

            var setting = new ConfigSetting
            {
                Id = Guid.NewGuid(),
                Key = model.Key,
                Value = model.Value,
                LanguageCode = model.LanguageCode,
                Description = model.Description
            };
            _settingService.Add(setting);
            _settingService.SaveChanges();
            return SettingList(command);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult SaveSetting(GridCommand command, SettingListModel model)
        {
            var setting = _settingService.GetById(model.Id);
            if (setting == null)
            {
                return Content("Setting does not exist");
            }

            if (_settingService.CheckKeyExists(model.Key,model.LanguageCode,model.Id))
            {
                return Content("Setting existed");
            }

            setting.Key = model.Key;
            setting.Value = model.Value;
            setting.LanguageCode = model.LanguageCode;
            setting.Description = model.Description;

            _settingService.SaveChanges();

            return SettingList(command);
        }

        [HttpPost]
        [GridAction(EnableCustomBinding = true)]
        public ActionResult DeleteSetting(GridCommand command, SettingListModel model)
        {
            var setting = _settingService.GetById(model.Id);
            if (setting == null)
            {
                return Content("Setting does not exist");
            }

            _settingService.Delete(setting);
            _settingService.SaveChanges();
            return SettingList(command);
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
                var model =
                    _settingService.GetAll().Select(
                        q => new SettingListModel()
                        {
                            Id = q.Id,
                            Key = q.Key,
                            Value = q.Value,
                            LanguageCode = q.LanguageCode,
                            Description = q.Description
                        }).ToList();
                return View(model);
        }
    }
}
