using System;
using System.ComponentModel.DataAnnotations;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class ResourceListModel
    {
        public Guid Id { get; set; }

        [Required]
        [ResourceDisplayName("Admin.Configuration.Resource.Fields.Title")] 
        public string Key { get; set; }

        [ResourceDisplayName("Admin.Configuration.Resource.Fields.Value")]
        public string Value { get; set; }

        [Required]
        [UIHint("Language")]
        [ResourceDisplayName("Admin.Configuration.Product.Fields.LangCode")]
        public string LangCode { get; set; }
    }

}