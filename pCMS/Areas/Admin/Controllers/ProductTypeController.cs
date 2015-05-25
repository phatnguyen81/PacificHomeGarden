using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    
    public class ProductTypeController : BaseAdminController
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        #region Ajax
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductAttributes(Guid id, GridCommand command)
        {
            var productType = _productTypeService.GetById(id);
            if (productType == null)
                throw new ArgumentException("Không kiếm thấy thuộc tính này", "productTypeId");

            var attributes = productType.ProductAttributes.OrderBy(x => x.DisplayOrder).ToList();

            var model = new GridModel<ProductAttributeModel>
            {
                Data = attributes.Select(x => new ProductAttributeModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    AllowFilter = x.AllowFilter,
                    DisplayOrder = x.DisplayOrder,
                    DataType = x.DataType
                }),
                Total = attributes.Count
            };
            return new JsonResult
            {
                Data = model
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductTypes(GridCommand command)
        {
            var productTypes = _productTypeService.SearchProductTypes(null , true, 0, 20);
            var model = new GridModel<ProductTypeModel>
            {
                Data = productTypes.Select(
                    q => new ProductTypeModel()
                    {
                        Id = q.Id,
                        Title = q.Title
                    }).ToList(),
                Total = productTypes.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }
        #endregion

        public ActionResult List(ProductTypeListModel model)
        {
            var productTypes = _productTypeService.SearchProductTypes(null , true, 0, 20);
            model.ProductTypes = new GridModel<ProductTypeModel>
            {
                Data = productTypes.Select(
                    q => new ProductTypeModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                    }).ToList(),
                Total = productTypes.TotalCount
            };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ProductTypeItemModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ProductTypeItemModel model, bool continueEditing)
        {
            try
            {

                var productType = new ProductType
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description
                };
                _productTypeService.Add(productType);
                _productTypeService.SaveChanges();
                SuccessNotification("Thêm mới loại sản phẩm '" + model.Title + "' thành công");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = productType.Id })
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
            ViewBag.SelectedTab = selectedTab;
            var productType = _productTypeService.GetById(id);
            if (productType == null) return RedirectToAction("List");

            var model = new ProductTypeItemModel()
            {
                Id = productType.Id,
                Title = productType.Title,
                Description = productType.Description
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(ProductTypeItemModel model, bool continueEditing)
        {
            try
            {
                var productType = _productTypeService.GetById(model.Id);
                if (productType == null) return RedirectToAction("List");

                productType.Title = model.Title;
                productType.Description = model.Description;

                _productTypeService.SaveChanges();

                SuccessNotification("Cập nhật loại sản phẩm '" + model.Title + "' thành công");
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
                var productType = _productTypeService.GetById(id);

                _productTypeService.Delete(id);
                _productTypeService.SaveChanges();

                SuccessNotification("Xóa loại sản phẩm '" + productType.Title + "' thành công");
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
