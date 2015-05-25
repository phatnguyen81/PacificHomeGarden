using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace pCMS.Admin.Models
{

    public class ProductAttributeItemModel
    {
        public ProductAttributeItemModel()
        {
            DisplayOrder = 0;
            AllowFilter = false;
            DataTypeList = new List<SelectListItem>
                               {
                                   new SelectListItem{Text = "Option", Value = "O"},
                                   new SelectListItem{Text = "String", Value = "C"},
                                   new SelectListItem{Text = "Number", Value = "N"}
                               };
        }

        public Guid Id { get; set; }

        public Guid ProductTypeId { get; set; }

        [Display(Name = "Loại sản phẩm")]
        public string ProductTypeTitle { get; set; }

        [Display(Name = "Tên")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Ghi chú")]
        public string Description { get; set; }

        [Display(Name = "Kiểu dữ liệu")]
        public string DataType { get; set; }

        [Display(Name = "Cho phép tìm kiếm")]
        public bool AllowFilter { get; set; }

        [UIHint("DisplayOrder")]
        [Display(Name = "Thứ tự")]
        public int DisplayOrder { get; set; }

        public IEnumerable<SelectListItem> DataTypeList { get; set; }
    }

    public class ProductAttributeOptionModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Giá trị")]
        public string DataValue { get; set; }

        [UIHint("DisplayOrder")]
        [Display(Name = "Thứ tự")]
        public int OptionDisplayOrder { get; set; }
    }
}