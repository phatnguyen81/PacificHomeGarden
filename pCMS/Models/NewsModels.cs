using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace pCMS.Models
{
    public class NewsPagingModel
    {
        public int? Page { get; set; }
        public string EmailAddress { get; set; }
        public string LastName { get; set; }
        public IPagedList<NewsListItemModels> SearchResults { get; set; }
        public string SearchButton { get; set; }
    }
    public class NewsListItemModels
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public string Title { get; set; }
        public string Quote { get; set; }
    }

    public class NewsDetailModel
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Body { get; set; }
    }
}