using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pCMS.Models
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
            OrderItems = new List<OrderItemModel>();
        }
        public List<OrderItemModel> OrderItems { get; set; }

        public decimal TotalPrice
        {
            get
            {
                if (OrderItems == null || OrderItems.Count <= 0) return 0;
                return OrderItems.Sum(q => q.UnitPrice*q.Qtty);
            }
        }

        public class OrderItemModel
        {
       
            [Editable(false)]
            [Display(Name = "Product")]
            public Guid ProductId { get; set; }

            [Editable(false)]
            [Display(Name = "Picture")]
            public Guid PictureId { get; set; }

            [UIHint("UInt32")]
            [Required]
            [Display(Name = "Qty")]
            public int Qtty { get; set; }

            [Editable(false)]
            public decimal UnitPrice { get; set; }

            [Editable(false)]
            public string ThumbnailPictureUrl { get; set; }

            [Editable(false)]
            public string PictureUrl { get; set; }

            [Editable(false)]
            public string ProductTitle { get; set; }

            [Editable(false)]
            public string PictureTitle { get; set; }
        }
    }
}