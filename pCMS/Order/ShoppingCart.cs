using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pCMS.Core;
using pCMS.Services;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;

namespace pCMS.Order
{
    public class ShoppingCart
    {
        private readonly IOrderService _orderService;
        private readonly IPictureService _pictureService;
        public decimal TotalPrice
        {
            get
            {
                if (OrderList == null || OrderList.Count <= 0) return 0;
                return OrderList.Sum(q => q.UnitPrice * q.Qtty);
            }
        }

        private List<OrderItem> _orderList;
        public List<OrderItem> OrderList
        {
            get
            {
                if (CurrentOrder != null && _orderList == null)
                {
                    _orderList = CurrentOrder.OrderDetails.Select(q => new OrderItem
                    {
                        PictureId = q.PictureId,
                        PictureTitle = q.Picture.Product_Picture.FirstOrDefault(a => a.ProductId == q.ProductId).Title,
                        PictureUrl = _pictureService.GetPictureUrl(q.PictureId),
                        Qtty = q.Qtty,
                        UnitPrice = q.UnitPrice,
                        ProductId = q.ProductId,
                        ProductTitle = q.Product.Title,
                        ThumbnailPictureUrl = _pictureService.GetPictureUrl(q.PictureId, 90),
                    }).ToList();
                }
                if(CurrentOrder == null)
                {
                    _orderList = new List<OrderItem>();
                }
                return _orderList;
            }
        }
        public ShoppingCart()
        {
            _orderService = DependencyResolver.Current.GetService<IOrderService>();
            _pictureService = DependencyResolver.Current.GetService<IPictureService>();
            //OrderList = new List<OrderItem>();
        }

        public Core.Order CurrentOrder 
        { 
            get
            {
                if(!HttpContext.Current.User.Identity.IsAuthenticated) return null;
                var currentOrder = _orderService.GetCurrentOrderWaiting(HttpContext.Current.User.Identity.Name);
                if(currentOrder == null)
                {
                    var order = new Core.Order
                                    {
                                        Id = Guid.NewGuid(),
                                        TotalPrice = 0,
                                        UserName = HttpContext.Current.User.Identity.Name,
                                        Status = "W",
                                        OrderDate = DateTime.Now
                                    };
                    _orderService.Add(order);

                    currentOrder = _orderService.GetCurrentOrderWaiting(HttpContext.Current.User.Identity.Name);
                }
                return currentOrder;
            } 
        }
        public void AddItem(OrderItem orderDetail)
        {
            if (CurrentOrder != null)
            {
                // for member
                var existItem = CurrentOrder.OrderDetails.FirstOrDefault(
                    q => q.PictureId == orderDetail.PictureId && q.ProductId == orderDetail.ProductId);
                if (existItem != null)
                {
                    existItem.Qtty += orderDetail.Qtty;

                }
                else
                {
                    //OrderList.Add(orderDetail);
                    var orderItem = new OrderDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            UnitPrice = orderDetail.UnitPrice,
                                            OrderId = CurrentOrder.Id,
                                            PictureId = orderDetail.PictureId,
                                            ProductId = orderDetail.ProductId,
                                            Qtty = orderDetail.Qtty
                                        };
                    CurrentOrder.OrderDetails.Add(orderItem);
                    CurrentOrder.TotalPrice = CurrentOrder.OrderDetails.Sum(q => q.Qtty*q.UnitPrice);
                    

                }
                _orderService.SaveChanges();
            }
            // for all

            var existSessionItem =
                OrderList.FirstOrDefault(
                    q => q.PictureId == orderDetail.PictureId && q.ProductId == orderDetail.ProductId);
            if (existSessionItem != null)
                existSessionItem.Qtty += orderDetail.Qtty;
            else OrderList.Add(orderDetail);
        }
        public void RemoveItem(Guid productId, Guid pictureId)
        {
            OrderList.Remove(OrderList.FirstOrDefault(q => q.PictureId == pictureId && q.ProductId == productId));

            //CurrentOrder.OrderDetails.Remove(
            //    CurrentOrder.OrderDetails.FirstOrDefault(q => q.PictureId == pictureId && q.ProductId == productId));
            _orderService.DeleteOrderItem(CurrentOrder.OrderDetails.FirstOrDefault(q => q.PictureId == pictureId && q.ProductId == productId));
            CurrentOrder.TotalPrice = CurrentOrder.OrderDetails.Sum(q => q.Qtty * q.UnitPrice);
            _orderService.SaveChanges();
        }
        public void UpdateNewQtty(Guid productId, Guid pictureId, int newQtty)
        {
            var itemSession = OrderList.FirstOrDefault(q => q.ProductId == productId && q.PictureId == pictureId);
            if (itemSession != null)
            {
                itemSession.Qtty = newQtty;
            }
            if(CurrentOrder != null)
            {
                var item = CurrentOrder.OrderDetails.FirstOrDefault(q => q.ProductId == productId && q.PictureId == pictureId);
                if (item != null)
                {
                    item.Qtty = newQtty;
                    _orderService.SaveChanges();
                }
            }
        }
        public void SubmitCart()
        {
            CurrentOrder.OrderDate = DateTime.Now;
            CurrentOrder.Status = "P";
            _orderService.SaveChanges();
        }
        public void ClearAll()
        {
            OrderList.Clear();
        }
    }
}