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
    public class ProductAttributeController : BaseAdminController
    {
        private IProductAttributeService _productAttributeService;

        public ProductAttributeController(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        #region Ajax
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult AttributeOptions(Guid id, GridCommand command)
        {
            var attribute = _productAttributeService.GetById(id);
            if (attribute == null)
                throw new ArgumentException("No poll found with the specified id", "pollId");

            var attributes = attribute.ProductAttributeOptions.OrderBy(x => x.DisplayOrder).ToList();

            var model = new GridModel<ProductAttributeOptionModel>
            {
                Data = attributes.Select(x => new ProductAttributeOptionModel()
                {
                    Id = x.Id,
                    DataValue = x.DataValue,
                    OptionDisplayOrder = x.DisplayOrder
                }),
                Total = attributes.Count
            };
            return new JsonResult
            {
                Data = model
            };
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult AttributeOptionUpdate(ProductAttributeOptionModel model, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult { Data = "error" };
            }
            var option = _productAttributeService.GetOptionById(model.Id);
            option.DataValue = model.DataValue;
            option.DisplayOrder = model.OptionDisplayOrder;
            _productAttributeService.SaveChanges();

            return AttributeOptions(option.ProductAttributeId, command);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult AttributeOptionAdd(Guid id, ProductAttributeOptionModel model, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult { Data = "error" };
            }
            var attribute = _productAttributeService.GetById(model.Id);
            if (attribute == null)
                throw new ArgumentException("No poll found with the specified id", "pollId");

            attribute.ProductAttributeOptions.Add(new ProductAttributeOption()
            {
                Id = Guid.NewGuid(),
                DataValue = model.DataValue,
                DisplayOrder = model.OptionDisplayOrder,
                ProductAttributeId = id
            });
            _productAttributeService.SaveChanges();

            return AttributeOptions(attribute.Id, command);
        }


        [GridAction(EnableCustomBinding = true)]
        public ActionResult AttributeOptionDelete(Guid id, GridCommand command)
        {
            var option = _productAttributeService.GetOptionById(id);
            var attributeId = option.ProductAttributeId;
            _productAttributeService.DeleteOption(option.Id);
            _productAttributeService.SaveChanges();

            return AttributeOptions(attributeId, command);
        }
        #endregion
        public ActionResult Create(Guid productTypeId)
        {
            var productType = _productAttributeService.GetById(productTypeId);
            var model = new ProductAttributeItemModel
                            {
                                ProductTypeId = productTypeId, 
                                ProductTypeTitle = productType.Title
                            };
            return View(model);
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ProductAttributeItemModel model, bool continueEditing)
        {
            try
            {
                var attribute = new ProductAttribute
                                    {
                                        Id= Guid.NewGuid(),
                                        Title = model.Title,
                                        AllowFilter = model.AllowFilter,
                                        DataType = model.DataType,
                                        DisplayOrder = model.DisplayOrder,
                                        ProductTypeId = model.ProductTypeId
                                    };
                _productAttributeService.Add(attribute);
                _productAttributeService.SaveChanges();
                SuccessNotification("Thêm mới thuộc tính thành công '" + model.Title + "' thành công");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = attribute.Id })
                            : RedirectToAction("Edit", "ProductType",
                                                new {id = model.ProductTypeId, selectedTab = "attribute"});
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            var attribute = _productAttributeService.GetById(id);
            var model = new ProductAttributeItemModel
            {
                ProductTypeId = attribute.ProductTypeId,
                ProductTypeTitle = attribute.ProductType.Title,
                Title = attribute.Title,
                AllowFilter = attribute.AllowFilter,
                DataType = attribute.DataType,
                DisplayOrder = attribute.DisplayOrder,
                Id = attribute.Id
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(ProductAttributeItemModel model, bool continueEditing)
        {
            try
            {
                var attribute = _productAttributeService.GetById(model.Id);
                attribute.Title = model.Title;
                attribute.AllowFilter = model.AllowFilter;
                attribute.DataType = model.DataType;
                attribute.DisplayOrder = model.DisplayOrder;
                _productAttributeService.SaveChanges();
                SuccessNotification("Cập nhật thuộc tính thành công '" + model.Title + "' thành công");
                return continueEditing
                                ? RedirectToAction("Edit", new { id = attribute.Id })
                                : RedirectToAction("Edit", "ProductType",
                                                    new { id = model.ProductTypeId, selectedTab = "attribute" });
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
                var attribute = _productAttributeService.GetById(id);

                _productAttributeService.Delete(id);
                _productAttributeService.SaveChanges();

                SuccessNotification("Xóa thuộc tính thành công '" + attribute.Title + "' thành công");
                return RedirectToAction("Edit", "ProductType",
                                        new { id = attribute.ProductTypeId, selectedTab = "attribute" });
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}
