using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;
using pCMS.Framework;

namespace pCMS.Admin.Controllers
{
    public class LanguageController : BaseAdminController
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;

        public LanguageController(ILanguageService languageService, ILocalizationService localizationService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
                var model =
                    _languageService.GetAll().Select(
                        q => new LanguageListModel()
                        {
                            Id = q.Id,
                            Title = q.Title,
                            Code = q.Code,
                            IsDefault = q.IsDefault
                        }).ToList();
                return View(model);
        }

        public ActionResult Create()
        {

            var model = new LanguageItemModel();
            return View(model);
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(LanguageItemModel model, bool continueEditing)
        {
            try
            {
                if (_languageService.CheckExists(model.Code))
                {
                    throw new Exception(_localizationService.GetResource("Admin.Configuration.Languages.Messages.Existed"));
                    
                }

                if (!_languageService.GetAll().Any())
                {
                    model.IsDefault = true;
                }
                else if(model.IsDefault)
                {
                    foreach (var configLanguage in _languageService.GetAll())
                    {
                        configLanguage.IsDefault = false;
                    }
                }

                var language = new ConfigLanguage
                {
                    Id = Guid.NewGuid(),
                    Code = model.Code,
                    Title = model.Title,
                    IsDefault = model.IsDefault
                };
                _languageService.Add(language);
                _languageService.SaveChanges();
                SuccessNotification(
                    string.Format(
                        _localizationService.GetResource(
                            "Admin.Configuration.Languages.Messages.Added"), language.Title));
                return continueEditing
                            ? RedirectToAction("Edit", new { id = language.Id })
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
            var language = _languageService.GetById(id);
            var model = new LanguageItemModel
            {
                Id = language.Id,
                Code = language.Code,
                Title = language.Title,
                IsDefault = language.IsDefault
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(LanguageItemModel model, bool continueEditing)
        {
            try
            {
                if (_languageService.GetAll().All(q => q.Code == model.Code))
                {
                    model.IsDefault = true;
                }
                else if (model.IsDefault)
                {
                    foreach (var configLanguage in _languageService.GetAll())
                    {
                        configLanguage.IsDefault = false;
                    }
                }

                var language = _languageService.GetById(model.Id);
                if (language == null) return RedirectToAction("List");

                if (_languageService.CheckExists(model.Code, language.Id))
                {
                    throw new Exception(_localizationService.GetResource("Admin.Configuration.Languages.Messages.Existed"));
                }

                language.Code = model.Code;
                language.Title = model.Title;
                language.IsDefault = model.IsDefault;
                _languageService.SaveChanges();
                SuccessNotification(
                    string.Format(
                        _localizationService.GetResource(
                            "Admin.Configuration.Languages.Messages.Updated"), language.Title));
                return continueEditing
                                ? RedirectToAction("Edit", new { id = language.Id })
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
                var language = _languageService.GetById(id);
                if(language.IsDefault)
                {
                    var otherLanguage = _languageService.GetByCode(language.Code);
                    if(otherLanguage != null)
                    {
                        otherLanguage.IsDefault = true;
                    }
                }

                _languageService.Delete(language);
                _languageService.SaveChanges();
                SuccessNotification(
                    string.Format(
                        _localizationService.GetResource(
                            "Admin.Configuration.Languages.Messages.Deleted"), language.Title));
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("Edit", new { id });
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ResourceList(GridCommand command, string langCode)
        {
            //var count = _localizationService.GetAllByLanguageCode(langCode).Count();
            //var a = command.FilterDescriptors;
            var resources =
                _localizationService.GetAllByLanguageCode(langCode).Select(
                    q => new ResourceListModel()
                             {
                                 Id = q.Id,
                                 Key = q.Key,
                                 Value = q.Value,
                                 LangCode = q.LanguageCode
                             }).ForCommand(command);
                            

            var model = new GridModel<ResourceListModel>
            {
                Data = resources.PagedForCommand(command),
                Total = resources.Count()
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult InsertResource(GridCommand command, ResourceListModel model, string langCode)
        {
            if (_localizationService.CheckKeyExists(model.Key, model.LangCode))
            {
                return Content(_localizationService.GetResource("Admin.Configuration.Languages.Resources.Messages.NotExist"));
            }

            var resource = new ConfigResource
            {
                Id = Guid.NewGuid(),
                Key = model.Key,
                Value = model.Value,
                LanguageCode = model.LangCode
            };
            _localizationService.Add(resource);
            _localizationService.SaveChanges();

            return ResourceList(command, langCode);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult SaveResource(GridCommand command, ResourceListModel model, string langCode)
        {

            var resource =  _localizationService.GetById(model.Id);
            if (resource == null)
            {
                return Content(_localizationService.GetResource("Admin.Configuration.Languages.Resources.Messages.NotExist"));
            }

            if (_localizationService.CheckKeyExists(model.Key, model.LangCode, resource.Id))
            {
                return Content(_localizationService.GetResource("Admin.Configuration.Languages.Resources.Messages.Existed"));
            }

            resource.Key = model.Key;
            resource.Value = model.Value;
            resource.LanguageCode = model.LangCode;

            _localizationService.SaveChanges();

            return ResourceList(command, langCode);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult DeleteResource(GridCommand command, ResourceListModel model, string langCode)
        {
            var resource = _localizationService.GetById(model.Id);
            if (resource == null)
            {
                return Content(_localizationService.GetResource("Admin.Configuration.Languages.Resources.Messages.NotExist"));
            }

            _localizationService.Delete(resource);
            _localizationService.SaveChanges();
            return ResourceList(command, langCode);
        }
        public ActionResult Resources(string id)
        {
                var model = new ResourcesModel { LanguageCode = id };
                try
                {
                    model.ListLanguage =
                        _languageService.GetAll().Select(q => new SelectListItem {Text = q.Title, Value = q.Code}).
                            ToList();
                }
                catch (Exception ex)
                {
                    ErrorNotification(ex);
                }
                
                return View(model);
        }
    }
}
