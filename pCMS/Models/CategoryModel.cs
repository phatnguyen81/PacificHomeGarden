using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pCMS.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        [Display(Name = "Tên")]
        public string Title { get; set; }

        [Display(Name = "Tên")]
        public string FullTitle { get; set; }

        [Display(Name = "Chuyên mục cha")]
        public string ParentTitle { get; set; }

        [Display(Name = "Loại sản phẩm")]
        public string ProductTypeTitle { get; set; }

        [Display(Name = "Alias")]
        public string Alias { get; set; }
    }

    public class CatalogSlideShow
    {
        public Guid Id { get; set; }

        [Display(Name = "Tên")]
        public string Title { get; set; }

        [Display(Name = "Alias")]
        public string Alias { get; set; }

        public List<CategoryPictureListModel> Pictures { get; set; }
    }

    public class CategoryPictureListModel
    {
        public Guid PictureId { get; set; }
        public string Title { get; set; }
        public string SeoFilename { get; set; }
        public string Description { get; set; }
        public string ThumbPictureUrl { get; set; }
        public string PictureUrl { get; set; }
    }
}