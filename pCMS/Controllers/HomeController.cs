using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using Telerik.Web.Mvc;
using pCMS.Core;
using pCMS.Core.Domain;
using pCMS.Core.Utils;
using pCMS.Framework;
using pCMS.Framework.Helpers;
using pCMS.Models;
using pCMS.Order;
using pCMS.Services;
using pCMS.Utils;
using pCMS.Models;

namespace pCMS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IEventService _eventService;
        private readonly IArticleService _articleService;
        private readonly IAlbumService _albumService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IWebHelper _webHelper;
        private readonly ISearchService _searchService;
        private readonly IPageService _pageService;

        public HomeController(ICategoryService categoryService, IEventService eventService, IArticleService articleService, IAlbumService albumService, IPictureService pictureService, IProductService productService, IOrderService orderService, IUserService userService, IWebHelper webHelper, ISearchService searchService, IPageService pageService)
        {
            _categoryService = categoryService;
            _eventService = eventService;
            _articleService = articleService;
            _albumService = albumService;
            _pictureService = pictureService;
            _productService = productService;
            _orderService = orderService;
            _userService = userService;
            _webHelper = webHelper;
            _searchService = searchService;
            _pageService = pageService;
        }
        private PageModel GetPageModel(string alias)
        {
            var page = _pageService.GetByAlias(alias);
            var model = new PageModel();
            if (page != null)
            {
                model.Title = page.Title;
                model.Alias = page.Alias;
                model.Body = StringHelpers.ConvertDbToDisplay(page.Body);
                model.MetaDescription = page.MetaDescription;
                model.MetaKeywords = page.MetaKeywords;
                model.MetaTitle = page.MetaTitle;
            }
            return model;
        }
        public ActionResult Index()
        {

            return View();
        }

        public PartialViewResult ProductCategoryMenu()
        {
            var model =
                _categoryService.GetAllWithOrder().Where(q => q.ShowInMenu).Select(
                    q => new CategoryModel()
                                {
                                    Id = q.Id.Value,
                                    ParentId = q.ParentId,
                                    Title = q.Title,
                                    FullTitle = q.FullTitle,
                                    Alias = q.Alias,
                                    ParentTitle = q.ParentTitle,
                                    ProductTypeTitle = q.ProductTypeTitle
                                }).ToList();
            return PartialView("~/Views/Home/ProductCategoryMenu.cshtml", model);
        }


        public ActionResult PageInline(string alias)
        {
            //return View();
            return PartialView(GetPageModel(alias));
        }

        public ActionResult Company()
        {
            //return View();
            return View(GetPageModel("company"));
        }

        public ActionResult HowToBuy()
        {
            //return View();
            return View(GetPageModel("how-to-buy-from-us"));
        }

        public ActionResult DesignsProductions()
        {
            //return View();
            return View(GetPageModel("designs-productions"));
        }

        public ActionResult Care()
        {
            //return View();
            return View(GetPageModel("care-of-our-products"));
        }

        public ActionResult TermsConditions()
        {
            //return View();
            return View(GetPageModel("terms-conditions"));
        }

        public ActionResult TradeShow(EventPagingModel model)
        {
            var pageIndex = model.Page ?? 1;

            model.SearchResults = _eventService
                .GetPublishedEvents()
                .Select(q => new EventListModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    DateBegin = q.DateBegin,
                    DateEnd = q.DateEnd,
                    Location = q.Location,
                    LocationLink = q.LocationLink,
                    City = q.City,
                    Booth = q.Booth
                }).ToPagedList(pageIndex, 5); ;

            return View(model);
        }

        public ActionResult News(NewsPagingModel model)
        {
                var pageIndex = model.Page ?? 1;

                model.SearchResults = _articleService
                    .GetPublishedByChannelId(new Guid("d8227ee4-c233-4d72-a6dc-af505ab67a7d"))
                    .Select(q => new NewsListItemModels
                                     {
                                         Id = q.Id,
                                         Alias = q.Alias,
                                         Quote = q.Quote,
                                         Title = q.Title
                                     }).ToPagedList(pageIndex, 4);;

                    return View(model);
        }

        public ActionResult NewsDetail(string id)
        {
            var news = _articleService.GetByAlias(id);
            if (news == null) return View("News");
            var model = new NewsDetailModel
                            {
                                Id=news.Id,
                                Alias = news.Alias,
                                Title = news.Title,
                                Body = news.Body,
                                PublishedDate = news.PublishedDate
                            };
            return View(model);
        }



        public ActionResult Customers(AlbumPagingModel model)
        {
            var albumId = AppSettings.CustommersAlbum;
            var album = _albumService.GetById(albumId);
            var pageIndex = model.Page ?? 1;
            model.SearchResults = album.Album_Picture
                .OrderBy(q => q.DisplayOrder)
                .ThenBy(q => q.Picture.SeoFilename)
                .Select(q => new AlbumPictureListModel
                                    {
                                        PictureId = q.PictureId,
                                        Description = q.Description,
                                        SeoFilename = q.Picture.SeoFilename,
                                        PictureUrl = _pictureService.GetPictureUrl(q.Picture),
                                        ThumbPictureUrl = _pictureService.GetPictureUrl(q.Picture, 90)
                                    }).ToPagedList(pageIndex, 15);
            return View(model);
        }


        public ActionResult ProductsInGardens(AlbumPagingModel model)
        {
            var albumId = AppSettings.ProductsInGardensAlbum;
            var album = _albumService.GetById(albumId);
            var pageIndex = model.Page ?? 1;
            model.SearchResults = album.Album_Picture
                .OrderBy(q => q.DisplayOrder)
                .ThenBy(q => q.Picture.SeoFilename)
                .Select(q => new AlbumPictureListModel
                                    {
                                        PictureId = q.PictureId,
                                        Description = q.Description,
                                        SeoFilename = q.Picture.SeoFilename ,
                                        PictureUrl = _pictureService.GetPictureUrl(q.Picture),
                                        ThumbPictureUrl = _pictureService.GetPictureUrl(q.Picture, 90)
                                    }).ToPagedList(pageIndex, 15);
            return View(model);
        }

        public ActionResult ProductsInStores(AlbumPagingModel model)
        {
            var albumId = AppSettings.ProductsInStoresAlbum;
            var album = _albumService.GetById(albumId);
            var pageIndex = model.Page ?? 1;
            model.SearchResults = album.Album_Picture
                .OrderBy(q => q.DisplayOrder)
                .ThenBy(q => q.Picture.SeoFilename)
                .Select(q => new AlbumPictureListModel
                                    {
                                        PictureId = q.PictureId,
                                        Description = q.Description,
                                        SeoFilename = q.Picture.SeoFilename,
                                        PictureUrl = _pictureService.GetPictureUrl(q.Picture),
                                        ThumbPictureUrl = _pictureService.GetPictureUrl(q.Picture, 90)
                                    }).ToPagedList(pageIndex, 15);
            return View(model);
        }
        public ActionResult SliderOnFactoryDirect(AlbumPagingModel model)
        {
            var albumId = AppSettings.SliderOnFactoryDirect;
            var album = _albumService.GetById(albumId);
            var pageIndex = model.Page ?? 1;
            model.SearchResults = album.Album_Picture
                .OrderBy(q => q.DisplayOrder)
                .ThenBy(q => q.Picture.SeoFilename)
                .Select(q => new AlbumPictureListModel
                                    {
                                        PictureId = q.PictureId,
                                        Description = q.Description,
                                        SeoFilename = q.Picture.SeoFilename,
                                        PictureUrl = _pictureService.GetPictureUrl(q.Picture),
                                        ThumbPictureUrl = _pictureService.GetPictureUrl(q.Picture, 90)
                                    }).ToPagedList(pageIndex, 15);
            return PartialView(model);
        }
        

        public ActionResult ContactUs()
        {
            Session["CaptchaImageText"] = GenerateRandomCode();
            return View(new ContactModel());
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Searchs(GridCommand command, string keywords)
        {
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                var results = _searchService.QueryContent(keywords, command.Page - 1, command.PageSize);
                var model = new GridModel<SearchModel.SearchItem>
                {
                    Data = results.Select(
                        q => new SearchModel.SearchItem
                        {
                            Id = q.Id,
                            Title = q.Title,
                            ParentId = q.ParentId,
                            Content = q.Content,
                            Type = q.Type
                        }).ToList(),
                    Total = results.TotalCount
                };
                return new JsonResult
                {
                    Data = model
                };
            }
            return View();

        }


        public ActionResult Search(SearchModel model)
        {
            if(!string.IsNullOrWhiteSpace(model.Keywords))
            {
                var results = _searchService.QueryContent(model.Keywords, 0, 20);
                model.SearchResults = new GridModel<SearchModel.SearchItem>
                {
                    Data = results.Select(
                        q => new SearchModel.SearchItem
                        {
                            Id = q.Id,
                            Title = q.Title,
                            ParentId = q.ParentId,
                            Content = q.Content,
                            Type = q.Type
                        }).ToList(),
                    Total = results.TotalCount
                };
                return View(model);
            }
            return View(model);
        }

        public ActionResult CategoryDetail(string id)
        {
            var category = _categoryService.GetByAlias(id);

            var model = new CategoryDetailModel
                            {
                                Alias = category.Alias,
                                Title = category.Title,
                                PictureUrl = _pictureService.GetPictureUrl(category.PictureId ?? Guid.Empty),
                                ChildCategories =
                                    _categoryService.GetAll().Where(q => q.ParentId == category.Id).OrderBy(
                                        q => q.DisplayOrder).Select(q => new ChildCategoryListModel
                                                                                {
                                                                                    Id = q.Id,
                                                                                    Alias = q.Alias,
                                                                                    PictureUrl =
                                                                                        _pictureService.GetPictureUrl
                                                                                        (q.PictureId ?? Guid.Empty,200),
                                                                                    Title = q.Title
                                                                                }).ToList()
                            };
            return View(model);
        }

        private bool IsGuid(string candidate, out Guid output)
        {
            var isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            bool isValid = false;
            output = Guid.Empty;
            if (candidate != null)
            {

                if (isGuid.IsMatch(candidate))
                {
                    output = new Guid(candidate);
                    isValid = true;
                }
            }
            return isValid;
        }

        public ActionResult ProductList(string id,ProductListModel model)
        {
            var category = _categoryService.GetByAlias(id);
            if (category == null) return View("Index");
            var pageIndex = model.Page ?? 1;
            model.CategoryAlias = id;
            model.CategoryTitle = category.Title;
            model.SearchResults = _productService.GetAllProductsByCategory(id).OrderBy(q=>q.Title)
                                    .Select(q => new ProductListModel.Product
                                                     {
                                                         Id = q.Id,
                                                         Alias = q.Alias,
                                                         Title = q.Title,
                                                         PictureUrl =
                                                             _pictureService.GetPictureUrl(
                                                                 q.Product_Picture.FirstOrDefault(p => p.IsAvatar) ==
                                                                 null ? null : q.Product_Picture.FirstOrDefault(p => p.IsAvatar).Picture, 100)
                                                     }).ToPagedList(pageIndex, 15);
            return View(model);
        }
        [Authorize]
        public ActionResult ProductDetail(string id, string cat, Guid? picid)
        {
            var product = _productService.GetByAlias(id);
            Guid productId;
            if(product == null && IsGuid(id , out productId))
            {
                product = _productService.GetById(new Guid(id));
            }
            if (product == null) return RedirectToAction("Index");

            var model = new ProductDetailModel
                            {
                                Id = product.Id,
                                Body = product.Body,
                                Title = product.Title,
                                CategoryAlias = cat,
                                Pictures = product.Product_Picture.OrderBy(q => q.DisplayOrder)
                                    .Select(q => new ProductDetailModel.Picture
                                                     {
                                                         Id = q.PictureId,
                                                         Title = q.Title,
                                                         Description = q.Description,
                                                         PictureThumbnailUrl =
                                                             _pictureService.GetPictureUrl(q.Picture, 90),
                                                         PictureUrl = _pictureService.GetPictureUrl(q.Picture)
                                                     }).ToList()
                            };
            if(picid != null && model.Pictures.Any(q=>q.Id == picid))
            {
                model.PictureIndex = model.Pictures.FindIndex(q => q.Id == picid);
            }
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactModel model)
        {

            if (model.SecurityCode != (string) Session["CaptchaImageText"])
            {
                ViewData.ModelState.AddModelError("SecurityCode", "Wrong security code");
            }
            else
            {
                try
                {

                    var subject = string.Format("A email from {0} ({1})", model.BusinessName, model.Email);
                    var body =
                        @"
                <table>
                    <tr>
                        <td style='width:100px'>
                            Resale #
                        </td>
                        <td>
                            <strong>
                                {0}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Business Name
                        </td>
                        <td>
                            <strong>
                                {1}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contact Person
                        </td>
                        <td>
                            <strong>
                                {2}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Title
                        </td>
                        <td>
                            <strong>
                                {3}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Phone Number
                        </td>
                        <td>
                            <strong>
                                {4}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Email Address
                        </td>
                        <td>
                            <strong>
                                {5}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Business Description
                        </td>
                        <td>
                            <strong>
                                {6}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Taxtpaper ID #
                        </td>
                        <td>
                            <strong>
                                {7}
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Message
                        </td>
                        <td>
                            <strong>
                                {8}
                            </strong>
                        </td>
                    </tr>
                </table>
            ";
                    var client = new SmtpClient {Timeout = 60000};
                    body = string.Format(body,
                        Server.HtmlEncode(model.Resale),
                        Server.HtmlEncode(model.BusinessName),
                        Server.HtmlEncode(model.ContactPerson),
                        Server.HtmlEncode(model.Title),
                        Server.HtmlEncode(model.PhoneNumber),
                        Server.HtmlEncode(model.Email),
                        Server.HtmlEncode(model.BusinessDescription),
                        Server.HtmlEncode(model.TaxpaperId),
                        Server.HtmlEncode(model.Message));

                    using (var message = new MailMessage("admin@pacifichomeandgarden.com", AppSettings.EmailReceive)
                                             {
                                                 Subject = subject,
                                                 Body = body,
                                                 IsBodyHtml = true
                                             })
                    {
                        client.Send(message);
                    }
                    TempData["SendContactResult"] = "Send mail success, we will contact you later !!!";
                    return RedirectToAction("ContactUs");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error : " + ex.GetBaseException().Message.Replace("\r\n", "").Replace("\"", "'");

                }
            }
            Session["CaptchaImageText"] = GenerateRandomCode();
            return View(model);
        }

        public ActionResult CatalogSlideShow(string id)
        {
            var category = _categoryService.GetByAlias(id);
            var model = new CatalogSlideShow
                            {
                                Id = category.Id,
                                Alias = category.Alias,
                                Title = category.Title,
                                Pictures = category
                                    .Category_Picture.Select(q => new CategoryPictureListModel
                                                                      {
                                                                          PictureId = q.PictureId,
                                                                          SeoFilename =
                                                                              q.Picture.
                                                                              SeoFilename,
                                                                          Title = q.Title,
                                                                          Description = q.Description,
                                                                          ThumbPictureUrl =
                                                                              _pictureService.
                                                                              GetPictureUrl(
                                                                                  q.PictureId, 90),
                                                                          PictureUrl =
                                                                              _pictureService.
                                                                              GetPictureUrl(
                                                                                  q.PictureId)
                                                                      }).ToList()
                            };
            //return View(IsMobileDevice() ? "Tn3Gallery" : "Tn3Gallery", model);
            //return View("Tn3Gallery", model)
            return View("Pikachoose", model);
        }

        [HttpPost]
        public ActionResult AddItem(Guid productId, Guid pictureId, int qtty)
        {
            try
            {
                if(!User.Identity.IsAuthenticated)
                {
                    throw new Exception("Please login before shopping!!!");
                }
                if(qtty <= 0)
                {
                    throw new Exception("Qtty must greater than 0!!!");
                }
                var product = _productService.GetById(productId);
                var productPicture = product.Product_Picture.FirstOrDefault(q => q.PictureId == pictureId);
                var item = new OrderItem
                {
                    ProductId = productId,
                    PictureId = pictureId,
                    ProductTitle = product.Title,
                    PictureTitle = productPicture.Title,
                    ThumbnailPictureUrl = _pictureService.GetPictureUrl(productPicture.Picture, 90),
                    PictureUrl = _pictureService.GetPictureUrl(productPicture.Picture),
                    Qtty = qtty,
                    UnitPrice = productPicture.Price
                };
                SessionManager.CurrentShoppingCard.AddItem(item);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.GetBaseException().Message
                });
            }

            return Json(new
                            {
                                success = true,
                                message = "",
                                countitem = SessionManager.CurrentShoppingCard.OrderList.Count == 0 ? 0 : SessionManager.CurrentShoppingCard.OrderList.Sum(q=>q.Qtty),
                                countproduct = SessionManager.CurrentShoppingCard.OrderList.Count
                            });
        }

        public ActionResult ShoppingCart()
        {
            return View(new ShoppingCartModel());
        }
        public string GetTableShoppingCart()
        {
            const string tableTemp = @"<table border='1' cellspacing='0' cellpadding='4'>
                            <tr>
                                <td><strong>Product</strong></td>
                                <td><strong>Picture Title</strong></td>
                                <td><strong>Picture</strong></td>
                                <td><strong>Quantity</strong></td>  
                                <td><strong>Unit Price</strong></td>
                            </tr>
                            {0}
                        </table>";
            const string rowTemp = @"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td><img src='{2}' alt='{1}'/></td>
                                <td>{3}</td>
                                <td>{4}</td>
                            </tr>
                        ";
            var rowresult = SessionManager
                .CurrentShoppingCard
                .OrderList
                .Aggregate(string.Empty,
                           (current, item) =>
                           current +
                           string.Format(rowTemp,
                                         item.ProductTitle,
                                         item.PictureTitle,
                                         _webHelper.GetRootUrl() +_pictureService.GetPictureUrl(item.PictureId, 200),
                                         item.Qtty, string.Format("{0:c}", item.UnitPrice)));
            return string.Format(tableTemp, rowresult);
        }

        public JsonResult GetTotalPrice()
        {
            return Json(
                new {TotalPrice = string.Format("{0:c}", SessionManager.CurrentShoppingCard.TotalPrice)},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ShoppingCart(FormCollection formCollection)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    throw new Exception("Please login before submit cart!!!");
                }
                if(SessionManager.CurrentShoppingCard.OrderList.Count <= 0)
                {
                    throw new Exception("Cart is empty!!!");
                }
                //var order = new Core.Order
                //                {
                //                    Id = Guid.NewGuid(),
                //                    OrderDate = DateTime.UtcNow,
                //                    Status = "P",
                //                    UserName = WorkContext.UserLoginInfo.UserName,
                //                    TotalPrice =
                //                        SessionManager.CurrentShoppingCard.OrderList.Sum(q => q.UnitPrice*q.Qtty)
                //                };
                //foreach (var item in SessionManager.CurrentShoppingCard.OrderList)
                //{
                //    var orderitem = new OrderDetail
                //                        {
                //                            Id = Guid.NewGuid(),
                //                            PictureId = item.PictureId,
                //                            ProductId = item.ProductId,
                //                            Qtty = item.Qtty,
                //                            UnitPrice = item.UnitPrice
                //                        };
                //    order.OrderDetails.Add(orderitem);
                //}
                //_orderService.Add(order);
                //_orderService.SaveChanges();
                SessionManager.CurrentShoppingCard.SubmitCart();
                var totalprice = SessionManager.CurrentShoppingCard.TotalPrice;
                if(!string.IsNullOrWhiteSpace(AppSettings.EmailReceive))
                {
                    try
                    {
                        var listOrderHtml = GetTableShoppingCart();
                        SessionManager.CurrentShoppingCard.ClearAll();
                        //var client = new SmtpClient { Timeout = 60000 };
                        
                        const string body = @"
                            <table>
                                <tr>
                                    <td style='width:100px'>
                                        User Name
                                    </td>
                                    <td>
                                        <strong>
                                            {0}
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Full Name
                                    </td>
                                    <td>
                                        <strong>
                                            {1}
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email
                                    </td>
                                    <td>
                                        <strong>
                                            {2}
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Phone Number
                                    </td>
                                    <td>
                                        <strong>
                                            {3}
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address
                                    </td>
                                    <td>
                                        <strong>
                                            {4}
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Total Price
                                    </td>
                                    <td>
                                        <strong>
                                            {5}
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan='2'>
                                        {6}
                                    </td>
                                </tr>
                            </table>
                        ";
                        var user = _userService.GetUser(WorkContext.UserLoginInfo.UserName);
                        EmailHelper.SendMail(AppSettings.EmailReceive, "New order from PacificHomeGarden",
                                             string.Format(body, user.UserName, user.FullName, user.Email,
                                                           user.PhoneNumber, user.Address,
                                                           string.Format("{0:c}", totalprice),
                                                           listOrderHtml));
                        //OrderSubmitToCustomer(user);
                        EmailHelper.SendMailWithSignature(user.Email, "Your Pacific Home and Garden Order", "SubmitOrderSuccess.htm");
                    }
                    catch
                    {
                        
                    }
                }
                
                SuccessNotification("Your cart has been submitted! We will contact you once your order has been processed.");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
            }
            return RedirectToAction("ShoppingCart");
        }

        public void OrderSubmitToCustomer(PCmsUser user)
        {
            try
            {
                const string body = @"Thank you for choosing Pacific Home and Garden! Your order is currently being processed. One of our sales representatives will be contacting you within 2 – 3 business days to confirm your order and to discuss any applicable discounts. If you have any questions, please don’t hesitate to contact us.
                            <br/>
                            Sincerely,
                            <br/>
                            <br/>
                            Pacific Home and Garden<br/>
                            678-518-8471 ext. 201 (East Coast)<br/>
                            510-259-0199 ext. 301 (West Coast)<br/>
                            info@pacifichomeandgarden.com";
                EmailHelper.SendMail(user.Email, "Your Pacific Home and Garden Order", body);
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
            }
            
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ShoppingCarts(GridCommand command)
        {
            var orderList = SessionManager.CurrentShoppingCard.OrderList.Select(
                                        q => new ShoppingCartModel.OrderItemModel
                                        {
                                            ProductId = q.ProductId,
                                            PictureId = q.PictureId,
                                            ProductTitle = q.ProductTitle,
                                            PictureTitle = q.PictureTitle,
                                            ThumbnailPictureUrl = q.ThumbnailPictureUrl,
                                            PictureUrl = q.PictureUrl,
                                            Qtty = q.Qtty,
                                            UnitPrice = q.UnitPrice
                                        }).ForCommand(command);

            var model = new GridModel<ShoppingCartModel.OrderItemModel>
            {
                Data = orderList.PagedForCommand(command),
                Total = orderList.Count()
            };

            return new JsonResult
            {
                Data = model
            };
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ShoppingCartUpdate(GridCommand command, ShoppingCartModel.OrderItemModel model)
        {
            //var item =
            //    SessionManager.CurrentShoppingCard.OrderList.FirstOrDefault(
            //        q => q.ProductId == model.ProductId && q.PictureId == model.PictureId);
            //if(item != null)
            //{
            //    item.Qtty = model.Qtty;
            //}
            SessionManager.CurrentShoppingCard.UpdateNewQtty(model.ProductId,model.PictureId,model.Qtty);

            return ShoppingCarts(command);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ShoppingCartDelete(GridCommand command, ShoppingCartModel.OrderItemModel model)
        {
            //SessionManager.CurrentShoppingCard.OrderList.Remove(
            //    SessionManager.CurrentShoppingCard.OrderList.FirstOrDefault(
            //        q => q.ProductId == model.ProductId && q.PictureId == model.PictureId));
            SessionManager.CurrentShoppingCard.RemoveItem(model.ProductId,model.PictureId);
            return ShoppingCarts(command);
        }

        public void CaptchaImage()
        {
            var ci = new CaptchaImage(
                Session["CaptchaImageText"].ToString(),
                60, 30, "Verdena");
            // Change the response headers to output a JPEG image.

            Response.Clear();
            Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.

            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.

            ci.Dispose();
        }

        private static string GenerateRandomCode()
        {
            const string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            var random = new Random();
            var s = "";
            for (var i = 0; i < 3; i++)
                s = String.Concat(s, charPool[random.Next(charPool.Length)]);
            return s;
        }

        private bool IsMobileDevice()
        {
            if (Request.UserAgent == null) return false;
            var strUserAgent = Request.UserAgent.ToString(CultureInfo.InvariantCulture).ToLower();
            return Request.Browser.IsMobileDevice || strUserAgent.Contains("iphone") ||
                   strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                   strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
                   strUserAgent.Contains("palm");
        }

        public ActionResult OderOnline()
        {
            return View(GetPageModel("order-online"));
        }
        //private static bool IsAndroidDevice()
        //{
        //    var useragent = Request.UserAgent;

        //    if (useragent != null)
        //    {
        //        if (useragent.ToLower().Contains(Constants..Android))
        //        {
        //            return true;
        //        }           
        //    }
        //    return false;
        //}
    }
}