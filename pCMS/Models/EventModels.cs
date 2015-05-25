using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;

namespace pCMS.Models
{
    public class EventPagingModel
    {
        public int? Page { get; set; }
        public IPagedList<EventListModel> SearchResults { get; set; }
    }
    public class EventListModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Event")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date Begin")]
        public DateTime DateBegin { get; set; }

        [Display(Name = "Date End")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Show Location")]
        public string Location { get; set; }

        public string LocationLink { get; set; }

        [Display(Name = "City, State")]
        public string City { get; set; }

        [Display(Name = "Booth #")]
        public string Booth { get; set; }

        [Display(Name = "Published?")]
        public bool IsPublished { get; set; }
    }

}