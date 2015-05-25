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
    public class PageController : BaseAdminController
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Pages(GridCommand command)
        {
            var pages = _pageService.SearchPages(null, true, command.Page - 1, command.PageSize);
            var model = new GridModel<PageModel>
            {
                Data = pages.Select(
                    q => new PageModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias,
                    }).ToList(),
                Total = pages.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }
        public ActionResult List(PageListModel model)
        {
            var pages = _pageService.SearchPages(model.Keywords, true, 0, 20);
            model.Pages = new GridModel<PageModel>
            {
                Data = pages.Select(
                    q => new PageModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias
                    }).ToList(),
                Total = pages.TotalCount
            };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new PageModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(PageModel model, bool continueEditing)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _pageService.CheckExistAlias(model.Alias))
                {
                    throw new Exception("Alias existed");
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
                    } while (_pageService.CheckExistAlias(model.Alias));

                }
                var page = new Page
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Alias = model.Alias,
                    Body = StringHelpers.ConvertContentToDb(model.Body),
                    ShowInSiteMap = model.ShowInSiteMap,
                    MetaKeywords = model.MetaKeywords,
                    MetaDescription = model.MetaDescription,
                    MetaTitle = model.MetaTitle
                };
                _pageService.Add(page);
                SuccessNotification("Add page '" + model.Title + "' successful");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = page.Id })
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
            var page = _pageService.GetById(id);
            if (page == null) return RedirectToAction("List");

            var model = new PageModel()
            {
                Id = page.Id,
                Title = page.Title,
                Alias = page.Alias,
                Body = StringHelpers.ConvertDbToDisplay(page.Body),
                ShowInSiteMap = page.ShowInSiteMap,
                MetaKeywords = page.MetaKeywords,
                MetaDescription = page.MetaDescription,
                MetaTitle = page.MetaTitle
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(PageModel model, bool continueEditing)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Alias) && _pageService.CheckExistAlias(model.Alias, model.Id))
                {
                    throw new Exception("Alias existed");
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
                    } while (_pageService.CheckExistAlias(model.Alias, model.Id));
                }
                var page = _pageService.GetById(model.Id);
                if (page == null) return RedirectToAction("List");

                page.Title = model.Title;
                page.Alias = model.Alias;
                page.Body = StringHelpers.ConvertContentToDb(model.Body);
                page.ShowInSiteMap = model.ShowInSiteMap;
                page.MetaKeywords = model.MetaKeywords;
                page.MetaDescription = model.MetaDescription;
                page.MetaTitle = model.MetaTitle;

                _pageService.SaveChanges();

                SuccessNotification("Update page '" + model.Title + "' successful");
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
                var page = _pageService.GetById(id);

                _pageService.Delete(id);

                SuccessNotification("Delete page '" + page.Title + "' successful");
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