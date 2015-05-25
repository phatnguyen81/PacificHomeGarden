using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace pCMS.Models
{
    public class ProductDetailModel
    {
        public Guid Id { get; set; }
        public string CategoryAlias { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int PictureIndex { get; set; }
        public List<Picture> Pictures { get; set; }
        
        public class Picture
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string PictureUrl { get; set; }
            public string PictureThumbnailUrl { get; set; }
        }
    }

    public class ProductListModel
    {
        public string CategoryAlias { get; set; }
        public string CategoryTitle { get; set; }
        public List<Product> Products;
        public int? Page { get; set; }
        public IPagedList<Product> SearchResults { get; set; }
        public class Product
        {
            public Guid Id { get; set; }
            public string Alias { get; set; }
            public string Title { get; set; }
            public string PictureUrl { get; set; }
        }
    }
}