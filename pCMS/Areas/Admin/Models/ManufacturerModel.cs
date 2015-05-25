using System;
using System.ComponentModel.DataAnnotations;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class ManufacturerListModel
    {
        public string Keywords { get; set; }
        public GridModel<ManufacturerModel> Manufacturers { get; set; }
    }

    public class ManufacturerModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.Alias")]
        public string Alias { get; set; }
    }

    public class ManufacturerItemModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.Catalog.Manufacturer.Fields.MetaTitle")]
        public string MetaTitle { get; set; }
    }
}