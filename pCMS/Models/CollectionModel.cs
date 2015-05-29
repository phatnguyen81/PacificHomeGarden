using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pCMS.Models
{
    public class CollectionListModel
    {
        public CollectionListModel()
        {
            Collections = new List<CollectionModel>();
        }
        public List<CollectionModel> Collections { get; set; }
    }
    public class CollectionModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Alias { get; set; }
        public string ShortDesciption { get; set; }

        public string FullDescription { get; set; }

        public string PictureUrl { get; set; }


        public Guid FileDownloadId { get; set; }
    }
}