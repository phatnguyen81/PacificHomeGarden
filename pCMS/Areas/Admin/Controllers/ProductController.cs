using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core.Domain;
using pCMS.Core.Utils;
using pCMS.Framework;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _pictureService;
        private readonly ISearchService _searchService;
        #endregion

        #region Ctors
        public ProductController(IProductService productService, IManufacturerService manufacturerService, ICategoryService categoryService, IPictureService pictureService, ISearchService searchService)
        {
            _productService = productService;
            _manufacturerService = manufacturerService;
            _categoryService = categoryService;
            _pictureService = pictureService;
            _searchService = searchService;
        }
        #endregion

        #region Ajax Methods
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Products(GridCommand command, string keywords, Guid categoryId)
        {
            var products = _productService.Search(null, categoryId, null, null, null, keywords, command.Page - 1, command.PageSize);
            var model = new GridModel<ProductModel>
                            {
                                Data = products
                                    .Select(q =>
                                                {
                                                    var productModel = new ProductModel
                                                                           {
                                                                               Id = q.Id,
                                                                               Alias = q.Alias,
                                                                               Title = q.Title,
                                                                               CategoryTitle =
                                                                                   string.Join(",",
                                                                                               q.Product_Category.Select
                                                                                                   (
                                                                                                       c =>
                                                                                                       c.Category.Title)),
                                                                               ManufacturerTitle = q.Manufacturer.Title,
                                                                               IsPublished = q.IsDeleted,
                                                                               IsDeleted = q.IsDeleted,
                                                                               UserCreated = q.UserCreated,
                                                                               DateCreated =
                                                                                   DateTimeHelpers.
                                                                                   ConvertUtcToUserTimeZone(
                                                                                       q.DateCreated),
                                                                           };
                                                    return productModel;
                                                }),
                                Total = products.TotalCount
                            };
           
            return new JsonResult
            {
                Data = model
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategories(GridCommand command, Guid productId)
        {

            var productCategories = _productService.GetAllProductCategory(productId);
            var productCategoryModel = productCategories
                .Select(x => new ProductItemModel.ProductCategoryModel()
                                 {
                                     ProductId = productId,
                                     CategoryId = x.CategoryId,
                                     CategoryTitle = x.Category.Title,
                                     DisplayOrder = x.DisplayOrder,
                                     IsFeatured = x.IsFeatured
                                 }).ForCommand(command);

                var model = new GridModel<ProductItemModel.ProductCategoryModel>
                {
                    Data = productCategoryModel.PagedForCommand(command),
                    Total = productCategoryModel.Count()
                };

                return new JsonResult
                {
                    Data = model
                };
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryAdd(GridCommand command, ProductItemModel.ProductCategoryModel model, Guid productId, Guid newCategoryId)
        {
            if (!_productService.CheckProductCategoryExists(productId, model.CategoryId))
            {
                var productCategory = new Product_Category()
                {
                    ProductId = model.ProductId,
                    CategoryId = newCategoryId,
                    IsFeatured = model.IsFeatured,
                    DisplayOrder = model.DisplayOrder
                };
                _productService.InsertProductCategory(productCategory);
                _productService.SaveChanges();
            }
            return ProductCategories(command, model.ProductId);
        }
        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryUpdate(GridCommand command, ProductItemModel.ProductCategoryModel model, Guid newCategoryId)
        {
            var productCategory = _productService.GetProductCategory(model.ProductId, model.CategoryId);
            if (productCategory == null)
                throw new ArgumentException("No found category");

            _productService.DeleteProductCategory(productCategory);
            var newProductCategory  = new Product_Category()
            {
                ProductId = model.ProductId,
                CategoryId = newCategoryId,
                IsFeatured = model.IsFeatured,
                DisplayOrder = model.DisplayOrder
            };
            _productService.InsertProductCategory(newProductCategory);
            _productService.SaveChanges();
            return ProductCategories(command, model.ProductId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryDelete(GridCommand command, ProductItemModel.ProductCategoryModel model)
        {
            var productCategory = _productService.GetProductCategory(model.ProductId, model.CategoryId);
            if (productCategory == null)
                throw new ArgumentException("No found category");

            _productService.DeleteProductCategory(productCategory);
            _productService.SaveChanges();
            return ProductCategories(command, model.ProductId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductPictures(GridCommand command, Guid productId)
        {
            var productPictures = _productService.GetAllPictures(productId);
            var productPictureListModel = productPictures
                .Select(x => new ProductItemModel.ProductPictureModel
                {
                    PictureId = x.Picture.Id,
                    ProductId = x.ProductId,
                    MineType = x.Picture.MimeType,
                    SeoFilename = x.Picture.SeoFilename,
                    IsAvatar = x.IsAvatar,
                    Title = x.Title,
                    Description = x.Description,
                    DisplayOrderPicture = x.DisplayOrder,
                    PictureUrl = _pictureService.GetPictureUrl(x.Picture),
                    Price = x.Price
                })
                .OrderBy(q => q.DisplayOrderPicture).ThenBy(q => q.SeoFilename).Skip((command.Page - 1)*command.PageSize).Take(command.PageSize).ToList();

            var model = new GridModel<ProductItemModel.ProductPictureModel>
            {
                Data = productPictureListModel,
                Total = productPictures.Count()
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductPictureDelete(GridCommand command, ProductItemModel.ProductPictureModel model)
        {
            var product = _productService.GetById(model.ProductId);
            product.Product_Picture.Remove(product.Product_Picture.FirstOrDefault(q => q.PictureId== model.PictureId));

            _pictureService.DeletePicture(model.PictureId);
            //_searchService.DeleteContent(model.PictureId);
            _productService.SaveChanges();
            return ProductPictures(command, model.ProductId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductPictureUpdate(GridCommand command, ProductItemModel.ProductPictureModel model)
        {

            var product = _productService.GetById(model.ProductId);
            var productPicture = product.Product_Picture.FirstOrDefault(q => q.PictureId == model.PictureId);
            if (productPicture != null)
            {
                if (model.IsAvatar)
                {
                    foreach (var picture in product.Product_Picture.Where(q=>q.PictureId != model.ProductId))
                    {
                        picture.IsAvatar = false;
                    }
                }

                productPicture.Description = Server.HtmlDecode(model.Description);
                productPicture.DisplayOrder = model.DisplayOrderPicture;
                productPicture.Title = model.Title;
                productPicture.IsAvatar = model.IsAvatar;
                _productService.SaveChanges();
            }
            return ProductPictures(command, model.ProductId);
        }

        public ActionResult UploadPictures(IEnumerable<HttpPostedFileBase> attachments1, Guid productId)
        {
            foreach (var file in attachments1)
            {
                var product = _productService.GetById(productId);
                var picture = _pictureService.InsertPicture(file.GetPictureBits(), file.ContentType,
                                                null, false);
                var picDefault = product.Product_Picture.FirstOrDefault(q => q.IsDefault);
                var productPicture = new Product_Picture
                                         {
                                             ProductId = product.Id,
                                             PictureId = picture.Id,
                                             DisplayOrder = 0
                                         };
                if(picDefault != null)
                {
                    productPicture.Description = picDefault.Description;
                }
                product.Product_Picture.Add(productPicture);

                _categoryService.SaveChanges();

                //_searchService.AddContent(new DocumentSearchItem
                //                              {
                //                                  Id = picture.Id,
                //                                  ParentId = product.Id,
                //                                  Title = productPicture.Title,
                //                                  Content = productPicture.Description,
                //                                  Keywords = string.Empty,
                //                                  Type = DocumentType.ProductPicture
                //                              });
            }
            return Content("");
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(ProductListModel model)
        {

            var products = _productService.Search(null, model.CategoryId, null, null, null, model.Keywords, 0, 20);
            model.Products
                = new GridModel<ProductModel>
                      {
                          Data = products
                              .Select(q =>
                                          {
                                              var productModel = new ProductModel
                                                                     {
                                                                         Id = q.Id,
                                                                         Alias = q.Alias,
                                                                         Title = q.Title,
                                                                         CategoryTitle =
                                                                             string.Join(",",
                                                                                         q.Product_Category.Select(
                                                                                             c => c.Category.Title)),
                                                                         ManufacturerTitle = q.Manufacturer.Title,
                                                                         IsPublished = q.IsDeleted,
                                                                         IsDeleted = q.IsDeleted,
                                                                         UserCreated = q.UserCreated,
                                                                         DateCreated =
                                                                             DateTimeHelpers.ConvertUtcToUserTimeZone(
                                                                                 q.DateCreated),
                                                                     };
                                              return productModel;
                                          }),
                          Total = products.TotalCount
                      };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ProductItemModel();
            PrepareProductItemModel(model);
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ProductItemModel model, bool continueEditing)
        {
            PrepareProductItemModel(model);
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _productService.CheckExistAlias(model.Alias))
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
                    } while (_productService.CheckExistAlias(model.Alias));
                }

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Alias = model.Alias,
                    MetaKeywords = model.MetaKeywords,
                    MetaDescription = model.MetaDescription,
                    MetaTitle = model.MetaTitle,
                    CallForPrice = model.CallForPrice,
                    IsPublished = model.IsPublished,
                    ManufacturerId = model.ManufacturerId,
                    Body = model.Body,
                    DateCreated = DateTime.UtcNow,
                    OldPrice = model.OldPrice,
                    Price = model.Price,
                    Quote = model.Quote,
                    UserCreated = WorkContext.UserLoginInfo.UserName
                };
                _productService.Add(product);
                _productService.SaveChanges();
                SuccessNotification("Create product '" + model.Title + "' successful");
                // Add Search
                //_searchService.UpdateContent(new DocumentSearchItem
                //{
                //    Id = product.Id,
                //    Title = product.Title,
                //    Content = product.Body,
                //    Type = DocumentType.News,
                //    Keywords = string.Join(",", product.Product_Picture.Select(q => q.Title))
                //});
                return continueEditing
                            ? RedirectToAction("Edit", new { id = product.Id })
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
            ViewBag.SelectedTab = Request["selectedTab"];
            var product = _productService.GetById(id);
            var model = new ProductItemModel
            {
                Alias = product.Alias,
                Title = product.Title,
                Id = product.Id,
                MetaDescription = product.MetaDescription,
                MetaKeywords = product.MetaKeywords,
                MetaTitle = product.MetaTitle,
                ManufacturerId = product.ManufacturerId,
                Quote = product.Quote,
                Body = product.Body,
                CallForPrice = product.CallForPrice,
                Price = product.Price,
                OldPrice = product.OldPrice,
                IsDeleted = product.IsDeleted,
                IsPublished = product.IsPublished,
                DateCreated = DateTimeHelpers.ConvertUtcToUserTimeZone(product.DateCreated),
                UserCreated = product.UserCreated
            };
            PrepareProductItemModel(model);
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(ProductItemModel model, bool continueEditing)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Alias) && _productService.CheckExistAlias(model.Alias, model.Id))
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
                    } while (_productService.CheckExistAlias(model.Alias, model.Id));
                }
                var product = _productService.GetById(model.Id);
                if (product == null) return RedirectToAction("List");

                product.Title = model.Title;
                product.Alias = model.Alias;
                product.ManufacturerId = model.ManufacturerId;
                product.IsPublished = model.IsPublished;
                product.Price = model.Price;
                product.OldPrice = model.OldPrice;
                product.CallForPrice = model.CallForPrice;
                product.Quote = model.Quote;
                product.Body = model.Body;
                product.MetaDescription = model.MetaDescription;
                product.MetaKeywords = model.MetaKeywords;
                product.MetaTitle = model.MetaTitle;
                _productService.SaveChanges();
                SuccessNotification("Update product '" + model.Title + "' successful !!!");
                // Update Search
                //_searchService.UpdateContent(new DocumentSearchItem
                //{
                //    Id = product.Id,
                //    Title = product.Title,
                //    Content = product.Body,
                //    Type = DocumentType.News,
                //    Keywords = string.Join(",", product.Product_Picture.Select(q => q.Title))
                //});
                return continueEditing
                                ? RedirectToAction("Edit", new { id = product.Id })
                                : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            PrepareProductItemModel(model);
            return View(model);

        }


        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var product = _productService.GetById(id);

                while (product.Product_Picture != null &&  product.Product_Picture.Count > 0)
                {
                    _pictureService.DeletePicture(product.Product_Picture.First().PictureId);
                }
                while (product.Product_Category != null && product.Product_Category.Count > 0)
                {
                    product.Product_Category.Remove(product.Product_Category.First());
                }
                _productService.Delete(id);
                _productService.SaveChanges();

                SuccessNotification("Delete product '" + product.Title + "' successful");

                //_searchService.DeleteContent(id);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, true);
            }
            return RedirectToAction("Edit", new { id });
        }

        public ActionResult BuildAll()
        {
            foreach (var product in _productService.GetAll())
            {
                foreach (var productPicture in product.Product_Picture)
                {
                    _searchService.AddContent(new DocumentSearchItem
                    {
                        Id = productPicture.Picture.Id,
                        ParentId = product.Id,
                        Title = productPicture.Title,
                        Content = productPicture.Description,
                        Type = DocumentType.ProductPicture,
                        Keywords = string.Empty
                    });
                }

            }
            return View();
        }
        #endregion

        #region Methods
        private void PrepareProductItemModel(ProductItemModel model)
        {
            model.Categories =
                _categoryService.GetAllWithOrder().Select(
                    q => new SelectListItem {Value = q.Id.ToString(), Text = q.FullTitle}).ToList();
            model.Manufacturers =
                _manufacturerService.GetAll().OrderBy(q => q.DisplayOrder).Select(
                    q => new SelectListItem {Value = q.Id.ToString(), Text = q.Title}).ToList();
        }

        public ActionResult EditPicture(Guid id, Guid productId)
        {
            var productPicture = _productService.GetAllPictures(productId).FirstOrDefault(q => q.PictureId == id);
            if (productPicture == null)
                return RedirectToAction("Edit", new { id = productId, selectedTab = "picture"});
            var model = new ProductItemModel.ProductPictureModel
                            {
                                PictureId = id,
                                ProductId = productId,
                                Description = productPicture.Description,
                                DisplayOrderPicture = productPicture.DisplayOrder,
                                IsAvatar = productPicture.IsAvatar,
                                MineType = productPicture.Picture.MimeType,
                                PictureUrl = _pictureService.GetPictureUrl(productPicture.Picture),
                                SeoFilename = productPicture.Picture.SeoFilename,
                                IsDefault = productPicture.IsDefault,
                                Title = productPicture.Title,
                                Price = productPicture.Price
                            };
            return View(model);
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult EditPicture(ProductItemModel.ProductPictureModel model, bool continueEditing)
        {
            var product = _productService.GetById(model.ProductId);
            if(product == null)
                return RedirectToAction("List");
            var productPicture = product.Product_Picture.FirstOrDefault(q => q.PictureId == model.PictureId);
            if (productPicture == null)
                return RedirectToAction("Edit", new { id = model.ProductId, selectedTab = "picture" });

            if (model.IsAvatar)
            {
                foreach (var picture in product.Product_Picture.Where(q => q.PictureId != model.ProductId))
                {
                    picture.IsAvatar = false;
                }
            }

            if (model.IsDefault)
            {
                foreach (var picture in product.Product_Picture.Where(q => q.PictureId != model.ProductId))
                {
                    picture.IsDefault = false;
                }
            }

            productPicture.Title = model.Title;
            productPicture.Description = model.Description;
            productPicture.IsAvatar = model.IsAvatar;
            productPicture.DisplayOrder = model.DisplayOrderPicture;
            productPicture.IsDefault = model.IsDefault;
            productPicture.Price = model.Price;
            _productService.SaveChanges();
            SuccessNotification("Update picture '" + model.Title + "' successful");
            //_searchService.UpdateContent(new DocumentSearchItem
            //{
            //    Id = productPicture.PictureId,
            //    ParentId = product.Id,
            //    Title = productPicture.Title,
            //    Content = productPicture.Description,
            //    Keywords = string.Empty,
            //    Type = DocumentType.ProductPicture
            //});
            return continueEditing
                        ? RedirectToAction("EditPicture", new { id = model.PictureId, productId = model.ProductId })
                        : RedirectToAction("Edit", new { id = model.ProductId, selectedTab = "picture" });
        }
        #endregion

    }
}
