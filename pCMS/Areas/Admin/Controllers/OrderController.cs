using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core.Utils;
using pCMS.Framework;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IPictureService _pictureService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        #endregion

        #region Ctors
        public OrderController(IOrderService orderService,
            IPictureService pictureService,
            IUserService userService,
            IProductService productService)
        {
            _orderService = orderService;
            _pictureService = pictureService;
            _userService = userService;
            _productService = productService;
        }
        #endregion

        #region Action Methods
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
           return View();
        }

        
        public ActionResult Edit(Guid id)
        {
            var order = _orderService.GetById(id);
            if(order == null) return RedirectToAction("List");
            var user = _userService.GetUser(order.UserName);
            var model = new OrderEditModel
                            {
                                Id = order.Id,
                                Email = user == null ? string.Empty : user.Email,
                                FullName = user == null ? string.Empty : user.FullName,
                                OrderDate = order.OrderDate,
                                UserName = order.UserName,
                                Address = user.Address,
                                PhoneNumber = user.PhoneNumber,
                                Status = order.Status == "A" ?"Approved" : "Pending",
                                OrderItems = order
                                    .OrderDetails
                                    .Select(q =>
                                                {
                                                    var productPicture =
                                                        _productService.GetProductPicture(
                                                            q.ProductId, q.PictureId);
                                                    return new OrderEditModel.OrderItemModel
                                                               {
                                                                   Id = q.Id,
                                                                   PictureId = q.PictureId,
                                                                   PictureTitle = productPicture.Title,
                                                                   ThumbnailPictureUrl = _pictureService.GetPictureUrl(productPicture.Picture, 90),
                                                                   PictureUrl =_pictureService.GetPictureUrl(productPicture.Picture),
                                                                   ProductId = q.ProductId,
                                                                   ProductTitle = q.Product.Title,
                                                                   Qtty = q.Qtty,
                                                                   UnitPrice = q.UnitPrice
                                                               };
                                                }).ToList()
                            };
            return View(model);
        }

        
        #endregion

        #region Ajax Methods
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var order = _orderService.GetById(id);
                while (order.OrderDetails.Count > 0)
                {
                    _orderService.DeleteOrderItem(order.OrderDetails.First());
                }
                _orderService.Delete(id);
                _orderService.SaveChanges();

                SuccessNotification("Delete order of '" + order.UserName + "' successful");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
            }
            return RedirectToAction("Edit", new { id });
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult OrderItems(GridCommand command, Guid id)
        {
            var order = _orderService.GetById(id);
            var orderList = order
                .OrderDetails
                .Select(q =>
                {
                    var productPicture =
                        _productService.GetProductPicture(
                            q.ProductId, q.PictureId);
                    return new OrderEditModel.OrderItemModel
                    {
                        Id = q.Id,
                        PictureId = q.PictureId,
                        PictureTitle = productPicture.Title,
                        ThumbnailPictureUrl =
                            _pictureService.GetPictureUrl(productPicture.Picture, 90),
                        PictureUrl = _pictureService.GetPictureUrl(productPicture.Picture),
                        ProductId = q.ProductId,
                        ProductTitle = q.Product.Title,
                        Qtty = q.Qtty,
                        UnitPrice = q.UnitPrice
                    };
                }).ForCommand(command);

            var model = new GridModel<OrderEditModel.OrderItemModel>
            {
                Data = orderList.PagedForCommand(command),
                Total = orderList.Count()
            };

            return new JsonResult
            {
                Data = model
            };
        }

        public string GetStatusName(string status)
        {
            switch (status)
            {
                case "A":
                    return "Approved";
                case "P":
                    return "Pending";
                default:
                    return "Waiting";
            }
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult OrderList(GridCommand command)
        {
            var orders = _orderService.GetAll(true).OrderByDescending(q => q.OrderDate);
            var orderListModel = orders
                .Select(x =>
                {
                    var user = _userService.GetUser(x.UserName);
                    return new OrderListModel
                               {
                                   UserName = x.UserName,
                                   FullName = (user == null ? x.UserName : user.FullName),
                                   Id = x.Id,
                                   Email = (user == null ? string.Empty : user.Email),
                                   NumOfItem =
                                       x.OrderDetails == null || x.OrderDetails.Count == 0
                                           ? 0
                                           : x.OrderDetails.Sum(q => q.Qtty),
                                   NumOfProduct = x.OrderDetails == null ? 0 : x.OrderDetails.Count,
                                   OrderDate = x.OrderDate,
                                   TotalPrice = x.TotalPrice,
                                   Status = GetStatusName(x.Status)
                               };
                }).ForCommand(command);

            var model = new GridModel<OrderListModel>
            {
                Data = orderListModel.PagedForCommand(command),
                Total = orderListModel.Count()
            };

            return new JsonResult
            {
                Data = model
            };
        }
        #endregion
    }
}
