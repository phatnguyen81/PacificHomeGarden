using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class CategoryListModel
    {
        public GridModel<CategoryModel> Categories { get; set; }
    }
    public class CategoryTreeModel
    {
        public List<CategoryModel> Categories { get; set; }
    }
    public class CategoryModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Title")]
        public string FullTitle { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.ParentCategory")]
        public string ParentTitle { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.ProductType")]
        public string ProductTypeTitle { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Alias")]
        public string Alias { get; set; }
    }


    public class CategoryItemModel
    {

        public CategoryItemModel()
        {
            ShowInMenu = true;
            Categories = new List<SelectListItem>();
            ProductTypes = new List<SelectListItem>();
        }

        public Guid Id { get; set; }


        [ResourceDisplayName("Admin.Catalog.Category.Fields.ParentCategory")]
        public Guid CategoryId { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.ProductType")]
        public Guid ProductTypeId { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Picture")]
        public Guid? PictureId { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.PictureUrl")]
        public string PictureUrl { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Description")]
        public string Description { get; set; }

        [UIHint("DisplayOrder")]
        [ResourceDisplayName("Admin.Catalog.Category.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.ShowInMenu")]
        public bool ShowInMenu { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.Catalog.Category.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> ProductTypes { get; set; }

        public class PictureListModel
        {

            public PictureListModel()
            {
                DisplayOrderPicture = 0;
            }

            [Editable(false)]
            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.Picture")]
            public Guid PictureId { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.Category")]
            public Guid CategoryId { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.SeoFilename")]
            public string SeoFilename { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.MineType")]
            public string MineType { get; set; }

            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.Title")]
            public string Title { get; set; }

            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.Description")]
            [UIHint("TinyRichEditor")]
            public string Description { get; set; }

            [Editable(false)]
            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.PictureUrl")]
            public string PictureUrl { get; set; }

            [UIHint("DisplayOrder")]
            [ResourceDisplayName("Admin.Catalog.CategoryPicture.Fields.DisplayOrder")]
            public int DisplayOrderPicture { get; set; }
        }
    }
}