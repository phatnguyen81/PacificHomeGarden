using System;
using System.ComponentModel.DataAnnotations;

namespace pCMS.Admin.Models
{
    public class SettingListModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        public string Value { get; set; }

        [Required]
        [UIHint("Language")]
        public string LanguageCode { get; set; }

        public string Description { get; set; }
    }

}