using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Core;
using System.Linq;
using pCMS.Framework;
using pCMS.Services;

namespace pCMS.Admin.Models
{
    public class ProductListModel
    {
        public ProductListModel()
        {
            var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
            Categories = categoryService.GetAllWithOrder().Select(q=> new SelectListItem{Text = q.FullTitle, Value = q.Id.Value.ToString()}).ToList();
            Categories.Insert(0, new SelectListItem {Text = "-- All --", Value = Guid.Empty.ToString()});
        }

        [ResourceDisplayName("Admin.Content.Product.Keywords")]
        public string Keywords { get; set; }
        [ResourceDisplayName("Admin.Content.Product.Category.Fields.Category")]
        public Guid CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }

        public GridModel<ProductModel> Products { get; set; }
    }
    public class ProductModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Category.Fields.CategoryTitle")]
        public string CategoryTitle { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Manufacturer.Fields.ManufacturerTitle")]
        public string ManufacturerTitle { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.IsDeleted")]
        public bool IsDeleted { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.UserCreated")]
        public string UserCreated { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.DateCreated")]
        public DateTime DateCreated { get; set; }
    }


    public class ProductItemModel
    {
        public ProductItemModel()
        {
            Categories = new List<SelectListItem>();
            Manufacturers = new List<SelectListItem>();
            IsPublished = false;
            CallForPrice = false;
            IsDeleted = false;
            Price = 0;
        }

        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.ManufacturerId")]
        public Guid ManufacturerId { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Quote")]
        public string Quote { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Body")]
        [AllowHtml]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.CallForPrice")]
        public bool CallForPrice { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.Price")]
        public decimal? Price { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.OldPrice")]
        public decimal? OldPrice { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.IsDeleted")]
        public bool IsDeleted { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.UserCreated")]
        public string UserCreated { get; set; }

        [ResourceDisplayName("Admin.Content.Product.Fields.DateCreated")]
        public DateTime DateCreated { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }

        public class ProductCategoryModel
        {
            public Guid ProductId { get; set; }
            public Guid CategoryId { get; set; }

            [AllowHtml]
            [ResourceDisplayName("Admin.Content.Product.Category.Fields.CategoryTitle")]
            [UIHint("ProductCategory")]
            public string CategoryTitle { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Category.Fields.IsFeatured")]
            public bool IsFeatured { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Category.Fields.DisplayOrder")]
            [UIHint("DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        public class ProductPictureModel
        {

            [Editable(false)]
            public Guid PictureId { get; set; }

            [Editable(false)]
            public Guid ProductId { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Fields.IsAvatar")]
            public bool IsAvatar { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Fields.IsDefault")]
            public bool IsDefault { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Fields.Price")]
            public decimal Price { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Content.Product.Fields.SeoFilename")]
            public string SeoFilename { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Content.Product.Fields.MineType")]
            public string MineType { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Fields.Title")]
            public string Title { get; set; }

            [ResourceDisplayName("Admin.Content.Product.Fields.Description")]
            [UIHint("RichEditor")]
            [AllowHtml]
            public string Description { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Content.Product.Fields.PictureUrl")]
            public string PictureUrl { get; set; }

            [UIHint("DisplayOrder")]
            [ResourceDisplayName("Admin.Content.Product.Fields.DisplayOrder")]
            public int DisplayOrderPicture { get; set; }
        }
    }


}