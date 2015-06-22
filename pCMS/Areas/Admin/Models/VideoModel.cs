using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class VideoListModel
    {
        public string Keywords { get; set; }

        public GridModel<VideoModel> Videos { get; set; }
    }
    public class VideoModel
    {
        public VideoModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Video.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Video.Fields.Picture")]
        [UIHint("Picture")]
        public Guid PictureId { get; set; }

        [ResourceDisplayName("Admin.Content.Video.Fields.VideoUrl")]
        public string VideoUrl { get; set; }

        [ResourceDisplayName("Admin.Content.Video.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [Required]
        [ResourceDisplayName("Admin.Content.Video.Fields.Category")]
        public Guid CategoryId { get; set; }

        [ResourceDisplayName("Admin.Content.Video.Fields.Category")]
        public string CategoryName { get; set; }

        public List<SelectListItem> AvailableCategories { get; set; } 
        
    }


}