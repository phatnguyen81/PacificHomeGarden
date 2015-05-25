using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class LanguageListModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Code")]
        public string Code { get; set; }

        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.IsDefault")]
        public bool IsDefault { get; set; }
    }

    public class ResourcesModel
    {
        public string LanguageCode { get; set; }
        public List<SelectListItem> ListLanguage { get; set; }
    }

    public class LanguageItemModel
    {
        public LanguageItemModel()
        {
            AllCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Except(CultureInfo.GetCultures(CultureTypes.SpecificCultures)).Select(
                    q => new SelectListItem { Text = (q.DisplayName + " (" + q.Name + ")"), Value = q.Name }).OrderBy(q => q.Text).ToList();
            IsDefault = false;
        }

        public Guid Id { get; set; }

        [Required]
        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Code")]
        public string Code { get; set; }

        [Required]
        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.IsDefault")]
        public bool IsDefault { get; set; }

        public List<SelectListItem> AllCultures { get; set; }
    }
}