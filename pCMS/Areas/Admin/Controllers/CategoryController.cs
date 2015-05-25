using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using pCMS.Admin.Models;
using pCMS.Core.Utils;
using pCMS.Framework;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _pictureService;
        private readonly IProductTypeService _productTypeService;
        #endregion

        #region Ctors
        public CategoryController(ICategoryService categoryService, IPictureService pictureService, IProductTypeService productTypeService)
        {
            _categoryService = categoryService;
            _pictureService = pictureService;
            _productTypeService = productTypeService;
        }
        #endregion

        #region Ajax methods

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult CategoryPictures(GridCommand command, Guid categoryId)
        {

            var categoryPictures = _categoryService.GetAllPictures(categoryId);
            var categoryPictureListModel = categoryPictures
                .Select(x => new CategoryItemModel.PictureListModel()
                                 {
                                     PictureId = x.Picture.Id,
                                     CategoryId = x.CategoryId,
                                     MineType = x.Picture.MimeType,
                                     SeoFilename = x.Picture.SeoFilename,
                                     Title = x.Title,
                                     Description = x.Description,
                                     DisplayOrderPicture = x.DisplayOrder,
                                     PictureUrl = _pictureService.GetPictureUrl(x.Picture)
                                 })
                .ForCommand(command);

            var model = new GridModel<CategoryItemModel.PictureListModel>
            {
                Data = categoryPictureListModel.PagedForCommand(command),
                Total = categoryPictureListModel.Count()
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult CategoryPictureDelete(GridCommand command, CategoryItemModel.PictureListModel model)
        {
            var category = _categoryService.GetById(model.CategoryId);
            var categoryProduct = category.Category_Picture.FirstOrDefault(q => q.PictureId == model.PictureId);
            category.Category_Picture.Remove(categoryProduct);

            _pictureService.DeletePicture(model.PictureId);

            _categoryService.SaveChanges();
            return CategoryPictures(command, model.CategoryId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult CategoryPictureUpdate(GridCommand command, CategoryItemModel.PictureListModel model)
        {

            var category = _categoryService.GetById(model.CategoryId);
            var categoryPicture = category.Category_Picture.FirstOrDefault(q => q.PictureId == model.PictureId);
            if (categoryPicture != null)
            {
                categoryPicture.Description = Server.HtmlDecode(model.Description);
                categoryPicture.DisplayOrder = model.DisplayOrderPicture;
                categoryPicture.Title = model.Title;
                _categoryService.SaveChanges();
            }
            return CategoryPictures(command, model.CategoryId);
        }

        public ActionResult UploadPictures(IEnumerable<HttpPostedFileBase> attachments1, Guid categoryId)
        {
            foreach (var file in attachments1)
            {
                var category = _categoryService.GetById(categoryId);
                //var picture = new Picture
                //{
                //    Id = Guid.NewGuid(),
                //    MimeType = file.ContentType,
                //    SeoFilename = file.FileName,
                //    PictureBinary = file.GetPictureBits(),
                //    IsNew = false
                //};
                var picture = _pictureService.InsertPicture(file.GetPictureBits(), file.ContentType,
                                                null, false);
                var categoryPicture = new Category_Picture
                {
                    CategoryId = category.Id,
                    PictureId = picture.Id,
                    DisplayOrder = 0
                };
                category.Category_Picture.Add(categoryPicture);

                _categoryService.SaveChanges();
            }
            return Content("");
        }

        public ActionResult UploadPicture(IEnumerable<HttpPostedFileBase> attachments, Guid categoryId)
        {
            var category = _categoryService.GetById(categoryId);

            if (category.PictureId != null)
            {
                var oldpicture = _pictureService.GetPictureById(category.PictureId.Value);
                _pictureService.DeletePicture(oldpicture);
            }

            var file = attachments.FirstOrDefault();
            var picture = _pictureService.InsertPicture(file.GetPictureBits(), file.ContentType, _pictureService.GetPictureSeoName(category.Title), false);

            category.PictureId = picture.Id;

            _categoryService.SaveChanges();
            return Json(new { status = "OK", pictureUrl = _pictureService.GetPictureUrl(picture) }, "text/plain");
        }

        public JsonResult DeletePicture(Guid categoryId)
        {
            var category = _categoryService.GetById(categoryId);
            if (category.PictureId != null)
            {
                var oldpicture = _pictureService.GetPictureById(category.PictureId.Value);
                _pictureService.DeletePicture(oldpicture);
                category.PictureId = null;
                _categoryService.SaveChanges();
                return Json(new { status = "OK", pictureUrl = _pictureService.GetPictureUrl(Guid.Empty) }, "text/plain");
            }
            return Json(new { status = "FAIL", errorMessage = "Không có hình để xóa" }, "text/plain");
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Categories(GridCommand command)
        {
            var categories = _categoryService.GetAllWithOrder(command.Page - 1, command.PageSize);
            var model = new GridModel<CategoryModel>
            {
                Data = categories.Select(
                        q => new CategoryModel
                        {
                            Id = q.Id.Value,
                            Title = q.Title,
                            FullTitle = q.FullTitle,
                            Alias = q.Alias,
                            ParentTitle = q.ParentTitle,
                            ProductTypeTitle = q.ProductTypeTitle
                        }).ToList(),
                Total = _categoryService.TotalCount()
            };

            return new JsonResult
            {
                Data = model
            };
        }
        [HttpPost]
        public ActionResult TreeLoadChildren(TreeViewItem node)
        {
            var parentId = !string.IsNullOrEmpty(node.Value) ? new Guid(node.Value) : Guid.Empty;

            var children = _categoryService.GetAllCategoriesByParentCategoryId(parentId).Select(x =>
                new TreeViewItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString(),
                    LoadOnDemand = _categoryService.GetAllCategoriesByParentCategoryId(x.Id).Count() > 0,
                    Enabled = true,
                    ImageUrl = Url.Content("~/Content/admin/images/ico-content.png")
                });

            return new JsonResult { Data = children };
        }

        #endregion

        #region Actions
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List(CategoryListModel model)
        {
            var categories = _categoryService.GetAllWithOrder(0, 20);
            model.Categories = new GridModel<CategoryModel>
                                   {
                                       Data = categories.Select(
                                           q => new CategoryModel
                                                    {
                                                        Id = q.Id.Value,
                                                        Title = q.Title,
                                                        FullTitle = q.FullTitle,
                                                        Alias = q.Alias,
                                                        ParentTitle = q.ParentTitle,
                                                        ProductTypeTitle = q.ProductTypeTitle
                                                    }).ToList(),
                                       Total = _categoryService.TotalCount()
                                   };
            return View(model);
        }
        public ActionResult Tree()
        {
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(null);
            var model = new CategoryTreeModel
                            {
                                Categories = categories.Select(q => new CategoryModel
                                                                        {
                                                                            Id = q.Id,
                                                                            Alias = q.Alias,
                                                                            Title = q.Title
                                                                        }).ToList()
                            };
            return View(model);
        }
        public ActionResult Create()
        {

            var model = new CategoryItemModel();
            PrepareCategoryItemModel(model);
            return View(model);
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(CategoryItemModel model, bool continueEditing)
        {
            PrepareCategoryItemModel(model);
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _categoryService.CheckExistAlias(model.Alias))
                {
                    throw new Exception("Alias exists");
                }
                var i = 0;
                do
                {
                    model.Alias = (i == 0 ? StringHelpers.MakeSEOTitle(model.Title) : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                    i++;
                } while (_categoryService.CheckExistAlias(model.Alias));


                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    ParentId = model.CategoryId == Guid.Empty ? (Guid?)null : model.CategoryId,
                    DisplayOrder = model.DisplayOrder,
                    ProductTypeId = model.ProductTypeId,
                    Title = model.Title,
                    Alias = model.Alias,
                    MetaKeywords = model.MetaKeywords,
                    MetaDescription = model.MetaDescription,
                    MetaTitle = model.MetaTitle,
                    Description = model.Description,
                    ShowInMenu = model.ShowInMenu
                };
                _categoryService.Add(category);
                _categoryService.SaveChanges();
                SuccessNotification("Add new category '" + model.Title + "' successful");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = category.Id })
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
            var category = _categoryService.GetById(id);
            var model = new CategoryItemModel
            {
                Alias = category.Alias,
                ProductTypeId = category.ProductTypeId,
                Title = category.Title,
                DisplayOrder = category.DisplayOrder,
                Id = category.Id,
                MetaDescription = category.MetaDescription,
                MetaKeywords = category.MetaKeywords,
                MetaTitle = category.MetaTitle,
                CategoryId = category.ParentId ?? Guid.Empty,
                Description = category.Description,
                PictureId = category.PictureId,
                PictureUrl = _pictureService.GetPictureUrl(category.PictureId ?? Guid.Empty),
                ShowInMenu = category.ShowInMenu
            };
            PrepareCategoryItemModel(model);
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(CategoryItemModel model, bool continueEditing)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Alias) && _categoryService.CheckExistAlias(model.Alias, model.Id))
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
                    } while (_categoryService.CheckExistAlias(model.Alias, model.Id));
                }
                var category = _categoryService.GetById(model.Id);
                if (category == null) return RedirectToAction("List");

                category.Title = model.Title;
                category.Alias = model.Alias;
                category.Description = model.Description;
                category.DisplayOrder = model.DisplayOrder;
                category.ProductTypeId = model.ProductTypeId;
                category.ParentId = model.CategoryId == Guid.Empty ? (Guid?)null : model.CategoryId;
                category.MetaDescription = model.MetaDescription;
                category.MetaKeywords = model.MetaKeywords;
                category.MetaTitle = model.MetaTitle;
                category.ShowInMenu = model.ShowInMenu;
                _categoryService.SaveChanges();
                SuccessNotification("Cập nhật chuyên mục '" + model.Title + "' thành công");
                return continueEditing
                                ? RedirectToAction("Edit", new { id = category.Id })
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
                var category = _categoryService.GetById(id);

                _categoryService.Delete(id);
                //_categoryService.SaveChanges();

                SuccessNotification("Delete category '" + category.Title + "' successful");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
            }
            return RedirectToAction("Edit", new { id });
        }
        #endregion

        #region methods
        public void PrepareCategoryItemModel(CategoryItemModel model)
        {
            model.Categories.Add(new SelectListItem { Text = "--Chọn--", Value = Guid.Empty.ToString() });
            if(model.Id == Guid.Empty)
            {
                model.Categories.AddRange(
                    _categoryService.GetAllWithOrder().Select(
                        q => new SelectListItem {Text = q.FullTitle, Value = q.Id.ToString()}).ToList());
            }
            else
            {
                model.Categories.AddRange(
                    _categoryService.GetAllExcludeNodeWithOrder(model.Id).Select(
                        q => new SelectListItem { Text = q.FullTitle, Value = q.Id.ToString() }).ToList());
            }

            model.ProductTypes =
                _productTypeService.GetAll().Select(
                    q => new SelectListItem { Value = q.Id.ToString(), Text = q.Title }).ToList();
        }
        #endregion

    }
}
