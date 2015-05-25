using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace pCMS.Models
{
    public class PageModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Alias { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public bool ShowInSiteMap { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }
    }
}