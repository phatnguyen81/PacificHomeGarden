using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace pCMS.Models
{
    public class AlbumPagingModel
    {
        public int? Page { get; set; }
        public IPagedList<AlbumPictureListModel> SearchResults { get; set; }
    }
    public class AlbumPictureListModel
    {
        public Guid PictureId { get; set; }
        public string SeoFilename { get; set; }
        public string Description { get; set; }
        public string ThumbPictureUrl { get; set; }
        public string PictureUrl { get; set; }
    }
}