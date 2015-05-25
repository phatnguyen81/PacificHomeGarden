using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core.Utils;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class ManufacturerController : BaseAdminController
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Manufacturers(GridCommand command)
        {
            var manufacturers = _manufacturerService.SearchManufacturers(null, true, command.Page - 1, command.PageSize);
            var model = new GridModel<ManufacturerModel>
            {
                Data = manufacturers.Select(
                    q => new ManufacturerModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias
                    }).ToList(),
                Total = manufacturers.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(ManufacturerListModel model)
        {
            var manufacturers = _manufacturerService.SearchManufacturers(model.Keywords, true, 0, 20);
            model.Manufacturers = new GridModel<ManufacturerModel>
            {
                Data = manufacturers.Select(
                    q => new ManufacturerModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias
                    }).ToList(),
                Total = manufacturers.TotalCount
            };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ManufacturerItemModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ManufacturerItemModel model, bool continueEditing)
        {
                try
                {

                    if (!string.IsNullOrWhiteSpace(model.Alias) && _manufacturerService.CheckExistAlias(model.Alias))
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
                        } while (_manufacturerService.CheckExistAlias(model.Alias));
                    }

                    var manufacturer = new Manufacturer
                    {
                        Id = Guid.NewGuid(),
                        Title = model.Title,
                        Alias = model.Alias,
                        MetaKeywords = model.MetaKeywords,
                        MetaDescription = model.MetaDescription,
                        MetaTitle = model.MetaTitle
                    };
                    _manufacturerService.Add(manufacturer);
                    _manufacturerService.SaveChanges();
                    SuccessNotification("Thêm mới nhà sản xuất '" + model.Title + "' thành công");
                    return continueEditing
                               ? RedirectToAction("Edit", new { id = manufacturer.Id })
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
            var manufacturer = _manufacturerService.GetById(id);
            if (manufacturer == null) return RedirectToAction("List");

            var model = new ManufacturerItemModel()
            {
                Id = manufacturer.Id,
                Title = manufacturer.Title,
                Alias = manufacturer.Alias,
                MetaKeywords = manufacturer.MetaKeywords,
                MetaDescription = manufacturer.MetaDescription,
                MetaTitle = manufacturer.MetaTitle
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(ManufacturerItemModel model, bool continueEditing)
        {
                try
                {

                    if (!string.IsNullOrWhiteSpace(model.Alias) && _manufacturerService.CheckExistAlias(model.Alias, model.Id))
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
                        } while (_manufacturerService.CheckExistAlias(model.Alias, model.Id));
                    }
                    var manufacturer = _manufacturerService.GetById(model.Id);
                    if (manufacturer == null) return RedirectToAction("List");

                    manufacturer.Title = model.Title;
                    manufacturer.Alias = model.Alias;
                    manufacturer.MetaKeywords = model.MetaKeywords;
                    manufacturer.MetaDescription = model.MetaDescription;
                    manufacturer.MetaTitle = model.MetaTitle;

                    _manufacturerService.SaveChanges();

                    SuccessNotification("Cập nhật nhà sản xuất '" + model.Title + "' thành công");
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
                    var manufacturer = _manufacturerService.GetById(id);

                    _manufacturerService.Delete(id);
                    _manufacturerService.SaveChanges();

                    SuccessNotification("Xóa nhà sản xuất '" + manufacturer.Title + "' thành công");
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
