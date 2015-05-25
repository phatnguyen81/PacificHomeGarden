using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Core.Utils;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class EventListModel
    {
        public string Keywords { get; set; }

        public GridModel<EventModel> Events { get; set; }
    }

    public class EventModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.Title")]
        public string Title { get; set; }

        [AllowHtml]
        [ResourceDisplayName("Admin.Content.Event.Fields.Description")]
        public string Description { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.DateBegin")]
        public DateTime DateBegin { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.DateEnd")]
        public DateTime DateEnd { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.ExpiredDate")]
        public DateTime? ExpiredDate { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.Location")]
        public string Location { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.City")]
        public string City { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.Booth")]
        public string Booth { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.IsPublished")]
        public bool IsPublished { get; set; }
    }

    public class EventItemModel
    {
        public EventItemModel()
        {
            IsPublished = true;
            PublishedDate = DateTimeHelpers.ConvertUtcToUserTimeZone(DateTime.UtcNow);
            DateBegin = DateTimeHelpers.Now;
            DateEnd = DateTimeHelpers.Now;
        }
            
        public Guid Id { get; set; }

        [Required]
        [ResourceDisplayName("Admin.Content.Event.Fields.Title")]
        public string Title { get; set; }

        [AllowHtml]
        [UIHint("RichEditor")]
        [ResourceDisplayName("Admin.Content.Event.Fields.Description")]
        public string Description { get; set; }

        [UIHint("Date")]
        [Required]
        [ResourceDisplayName("Admin.Content.Event.Fields.DateBegin")]
        public DateTime DateBegin { get; set; }

        [UIHint("Date")]
        [Required]
        [ResourceDisplayName("Admin.Content.Event.Fields.DateEnd")]
        public DateTime DateEnd { get; set; }

        [Required]
        [UIHint("DateTime")]
        [ResourceDisplayName("Admin.Content.Event.Fields.PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [UIHint("DateTimeNullable")]
        [ResourceDisplayName("Admin.Content.Event.Fields.ExpiredDate")]
        public DateTime? ExpiredDate { get; set; }


        [ResourceDisplayName("Admin.Content.Event.Fields.Location")]
        public string Location { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.LocationLink")]
        public string LocationLink { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.City")]
        public string City { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.Booth")]
        public string Booth { get; set; }

        [ResourceDisplayName("Admin.Content.Event.Fields.IsPublished")]
        public bool IsPublished { get; set; }
    }
}