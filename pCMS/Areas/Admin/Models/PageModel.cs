using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class PageListModel
    {
        public string Keywords { get; set; }

        public GridModel<PageModel> Pages { get; set; }
    }
    public class PageModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Page.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Page.Fields.Alias")]
        public string Alias { get; set; }

        [UIHint("RichEditor")]
        [ResourceDisplayName("Admin.Content.Page.Fields.Body")]
        [AllowHtml]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.Content.Page.Fields.ShowInSiteMap")]
        public bool ShowInSiteMap { get; set; }

        [ResourceDisplayName("Admin.Content.Page.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.Content.Page.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.Content.Page.Fields.MetaTitle")]
        public string MetaTitle { get; set; }
    }
}