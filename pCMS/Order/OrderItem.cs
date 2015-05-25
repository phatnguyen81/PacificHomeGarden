using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pCMS.Order
{
    public class OrderItem
    {
        
        public Guid ProductId { get; set; }
        public Guid PictureId { get; set; }
        public int Qtty { get; set; }
        public string ThumbnailPictureUrl { get; set; }
        public string PictureUrl { get; set; }
        public string ProductTitle { get; set; }
        public string PictureTitle { get; set; }
        public decimal UnitPrice { get; set; }
    }
}