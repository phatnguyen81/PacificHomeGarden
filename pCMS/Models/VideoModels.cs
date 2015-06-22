using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pCMS.Models
{
    public class VideoCategoryModel
    {
        public VideoCategoryModel()
        {
            Videos = new List<VideoModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<VideoModel> Videos { get; set; } 
    }
    public class VideoModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public string PictureUrl { get; set; }


    }
}