using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class OrderListModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.OrderDate")]
        public DateTime OrderDate { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.UserName")]
        public string UserName { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.FullName")]
        public string FullName { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.Email")]
        public string Email { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.NumOfProduct")]
        public int NumOfProduct { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.NumOfItem")]
        public int NumOfItem { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.TotalPrice")]
        public decimal TotalPrice { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.Status")]
        public string Status { get; set; }
    }

    public class OrderEditModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.OrderDate")]
        public DateTime OrderDate { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.UserName")]
        public string UserName { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.FullName")]
        public string FullName { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.Email")]
        public string Email { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [ResourceDisplayName("Admin.Sale.Order.Fields.Address")]
        public string Address { get; set; }

        public List<OrderItemModel> OrderItems { get; set; }

        public string Status { get; set; }

        public class OrderItemModel
        {
            public Guid Id { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.Product")]
            public Guid ProductId { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.Picture")]
            public Guid PictureId { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.Qtty")]
            public int Qtty { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.UnitPrice")]
            public decimal UnitPrice { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.ThumbnailPictureUrl")]
            public string ThumbnailPictureUrl { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.PictureUrl")]
            public string PictureUrl { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.Product")]
            public string ProductTitle { get; set; }

            [ResourceDisplayName("Admin.Sale.Order.Fields.Picture")]
            public string PictureTitle { get; set; }

        }
    }
}