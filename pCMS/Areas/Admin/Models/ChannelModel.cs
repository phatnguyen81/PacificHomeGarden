using System;
using System.ComponentModel.DataAnnotations;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class ChannelListModel
    {
        public string Keywords { get; set; }

        public GridModel<ChannelModel> Channels { get; set; }
    }
    public class ChannelModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.Alias")]
        public string Alias { get; set; }
    }
    public class ChannelItemModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.Content.Channel.Fields.MetaTitle")]
        public string MetaTitle { get; set; }
    }
}