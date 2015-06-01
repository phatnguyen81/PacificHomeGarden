using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pCMS.Models
{
    public class VideoModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public string PictureUrl { get; set; }


    }
}