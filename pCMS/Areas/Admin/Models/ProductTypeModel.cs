using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class ProductTypeListModel
    {
        public GridModel<ProductTypeModel> ProductTypes { get; set; }

    }

    public class ProductTypeModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Catalog.ProductType.Fields.Title")]
        public string Title { get; set; }

    }

    public class ProductTypeItemModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Catalog.ProductType.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Catalog.ProductType.Fields.Description")]
        public string Description { get; set; }
    }

    public class ProductAttributeModel
    {
        public ProductAttributeModel()
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

        [ResourceDisplayName("Admin.Catalog.ProductAttribute.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Catalog.ProductAttribute.Fields.DataType")]
        public string DataType { get; set; }

        [ResourceDisplayName("Admin.Catalog.ProductAttribute.Fields.DataTypeName")]
        public string DataTypeName
        {
            get { return DataTypeList.FirstOrDefault(q => q.Value == DataType).Text; }
        }

        [ResourceDisplayName("Admin.Catalog.ProductAttribute.Fields.AllowFilter")]
        public bool AllowFilter { get; set; }

        [ResourceDisplayName("Admin.Catalog.ProductAttribute.Fields.DisplayOrder")]
        [UIHint("DisplayOrder")]
        public int DisplayOrder { get; set; }



        public IEnumerable<SelectListItem> DataTypeList { get; set; }
    }

}