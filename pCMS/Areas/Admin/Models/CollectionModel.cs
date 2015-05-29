using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Core.Utils;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class CollectionListModel
    {
        public CollectionListModel()
        {
         
        }

        public GridModel<CollectionModel> Collections { get; set; } 
   
    }
    public class CollectionModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Collection.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Collection.Fields.Alias")]
        public string Alias { get; set; }
        [AllowHtml]
        [ResourceDisplayName("Admin.Content.Collection.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [ResourceDisplayName("Admin.Content.Collection.Fields.FullDescription")]
        public string FullDescription { get; set; }

        [ResourceDisplayName("Admin.Content.Collection.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ResourceDisplayName("Admin.Content.Collection.Fields.FileDownload")]
        public Guid FileDownloadId { get; set; }

        [ResourceDisplayName("Admin.Content.Collection.Fields.Picture")]
        public Guid PictureId { get; set; }

        public string FileName { get; set; }

        public bool DeleteFile { get; set; }
       
    }
    


}