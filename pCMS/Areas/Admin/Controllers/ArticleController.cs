using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core.Utils;
using pCMS.Framework;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class ArticleController : BaseAdminController
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        #region Ajax
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ArticleChannelList(GridCommand command, Guid articleId)
        {
            var channelArticles = _articleService.GetChannelArticleByArticleId(articleId);
            var articleChannelModel = channelArticles
                .Select(x => new ArticleItemModel.ArticleChannelModel
                                 {
                                     ArticleId = articleId,
                                     ChannelId = x.ChannelId,
                                     ChannelTitle = x.Channel.Title,
                                     ArticleChannelIsFeatured = x.IsFeatured,
                                 })
                .ForCommand(command);

                var model = new GridModel<ArticleItemModel.ArticleChannelModel>
                {
                    Data = articleChannelModel.PagedForCommand(command),
                    Total = articleChannelModel.Count()
                };

                return new JsonResult
                {
                    Data = model
                };
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ArticleChannelInsert(GridCommand command, ArticleItemModel.ArticleChannelModel model, Guid newChannelId)
        {
            if (_articleService.CheckChannelArticleExists(newChannelId, model.ArticleId))
            {
                return Content("Channel is existed");
            }

            var channelArticle = new ChannelArticle()
            {
                ChannelId = newChannelId,
                ArticleId = model.ArticleId,
                IsFeatured = model.ArticleChannelIsFeatured,

            };
            _articleService.InsertChannelArticle(channelArticle);
            return ArticleChannelList(command, model.ArticleId);
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ArticleChannelUpdate(GridCommand command, ArticleItemModel.ArticleChannelModel model, Guid newChannelId)
        {
            var channelArticle = _articleService.GetChannelArticle(model.ChannelId, model.ArticleId);
            if (channelArticle == null)
                return Content("Channel does not existed");

            _articleService.DeleteChannelArticle(channelArticle);
            var newChannelArticle = new ChannelArticle
                                        {
                                            ChannelId = newChannelId,
                                            ArticleId = model.ArticleId,
                                            IsFeatured = model.ArticleChannelIsFeatured
                                        };
            _articleService.InsertChannelArticle(newChannelArticle);
            return ArticleChannelList(command, model.ArticleId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ArticleChannelDelete(ArticleItemModel.ArticleChannelModel model, GridCommand command)
        {
            var channelArticle = _articleService.GetChannelArticle(model.ChannelId, model.ArticleId);
            if (channelArticle == null)
            {
                return Content("Channel does not existed");
            }

            _articleService.DeleteChannelArticle(channelArticle);
            return ArticleChannelList(command, model.ArticleId);
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Articles(GridCommand command, string keywords, string published, string feature, string username)
        {
            var articles = _articleService.SearchArticles(keywords,
                string.IsNullOrWhiteSpace(published) || published == "All" ? (bool?)null : Convert.ToBoolean(published),
                string.IsNullOrWhiteSpace(feature) || feature == "All" ? (bool?)null : Convert.ToBoolean(feature), 
                username, command.Page -1, command.PageSize);
            var model = new GridModel<ArticleModel>
            {
                Data = articles.Select(
                    q => new ArticleModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias,
                        Quote = q.Quote,
                        Body = q.Body,
                        IsPublished = q.IsPublished,
                        CreatedUser = q.CreatedUser,
                        ModifiedUser = q.ModifiedUser,
                        DeletedUser = q.DeletedUser,
                        CreatedDate =
                            DateTimeHelpers.ConvertUtcToUserTimeZone(q.CreatedDate),
                        ModifiedDate =
                            DateTimeHelpers.ConvertUtcToUserTimeZone(q.ModifiedDate),
                        DeletedDate =
                            DateTimeHelpers.ConvertUtcToUserTimeZone(q.DeletedDate),
                        ExpiredDate =
                            DateTimeHelpers.ConvertUtcToUserTimeZone(q.ExpiredDate),
                        IsDeleted = q.IsDeleted,
                        IsFeature = q.IsFeature
                    }).ToList(),
                Total = articles.TotalCount
            };
            return new JsonResult
                       {
                           Data = model
                       };
        }
        #endregion

        public ActionResult List(ArticleListModel model)
        {
            var published = string.IsNullOrWhiteSpace(model.Published) || model.Published == "All" ? (bool?)null :Convert.ToBoolean(model.Published);
            var featured = string.IsNullOrWhiteSpace(model.Featured) || model.Featured == "All" ? (bool?)null : Convert.ToBoolean(model.Featured);
            var articles = _articleService.SearchArticles(model.Keywords, published, featured,
                                                          model.UserName, 0, 20);
            model.Articles = new GridModel<ArticleModel>
                                 {
                                     Data = articles.Select(
                                         q => new ArticleModel
                                                  {
                                                      Id = q.Id,
                                                      Title = q.Title,
                                                      Alias = q.Alias,
                                                      Quote = q.Quote,
                                                      Body = q.Body,
                                                      IsPublished = q.IsPublished,
                                                      CreatedUser = q.CreatedUser,
                                                      ModifiedUser = q.ModifiedUser,
                                                      DeletedUser = q.DeletedUser,
                                                      CreatedDate =
                                                          DateTimeHelpers.ConvertUtcToUserTimeZone(q.CreatedDate),
                                                      ModifiedDate =
                                                          DateTimeHelpers.ConvertUtcToUserTimeZone(q.ModifiedDate),
                                                      DeletedDate =
                                                          DateTimeHelpers.ConvertUtcToUserTimeZone(q.DeletedDate),
                                                      ExpiredDate =
                                                          DateTimeHelpers.ConvertUtcToUserTimeZone(q.ExpiredDate),
                                                      IsDeleted = q.IsDeleted,
                                                      IsFeature = q.IsFeature
                                                  }).ToList(),
                                                  Total = articles.TotalCount
                                 };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ArticleItemModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ArticleItemModel model, bool continueEditing)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _articleService.CheckExistAlias(model.Alias))
                {
                    throw new Exception("Alias này đã tồn tại");
                }

                if (string.IsNullOrWhiteSpace(model.Alias))
                {
                    var i = 0;
                    do
                    {
                        model.Alias = (i == 0
                                           ? StringHelpers.MakeSEOTitle(model.Title)
                                           : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                        i++;
                    } while (_articleService.CheckExistAlias(model.Alias));
                }

                var article = new Article()
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Alias = model.Alias,
                    Quote = model.Quote,
                    Body = Server.HtmlDecode(model.Body),
                    CreatedDate = DateTime.UtcNow,
                    CreatedUser = WorkContext.UserLoginInfo.UserName,
                    IsFeature = model.IsFeature,
                    IsPublished = model.IsPublished,
                    PublishedDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.PublishedDate),
                    ExpiredDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.ExpiredDate),

                    MetaKeywords = model.MetaKeywords,
                    MetaDescription = model.MetaDescription,
                    MetaTitle = model.MetaTitle
                };
                _articleService.Add(article);
                SuccessNotification("Add article '" + model.Title + "' successful");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = article.Id })
                            : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }

            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            try
            {
                var article = _articleService.GetById(id);
                if (article == null) return RedirectToAction("List");

                var model = new ArticleItemModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Alias = article.Alias,
                    Quote = article.Quote,
                    Body = article.Body,
                    CreatedDate = DateTimeHelpers.ConvertUtcToUserTimeZone(article.CreatedDate),
                    DeletedDate = DateTimeHelpers.ConvertUtcToUserTimeZone(article.DeletedDate),
                    ExpiredDate =  DateTimeHelpers.ConvertUtcToUserTimeZone(article.ExpiredDate),
                    PublishedDate =  DateTimeHelpers.ConvertUtcToUserTimeZone(article.PublishedDate),
                    ModifiedDate =  DateTimeHelpers.ConvertUtcToUserTimeZone(article.ModifiedDate),
                    CreatedUser = article.CreatedUser,
                    ModifiedUser = article.ModifiedUser,
                    DeletedUser = article.DeletedUser,
                    IsFeature = article.IsFeature,
                    IsDeleted = article.IsDeleted,
                    IsPublished = article.IsPublished,
                    MetaKeywords = article.MetaKeywords,
                    MetaDescription = article.MetaDescription,
                    MetaTitle = article.MetaTitle
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
                return RedirectToAction("List");
            }
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(ArticleItemModel model, bool continueEditing)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _articleService.CheckExistAlias(model.Alias, model.Id))
                {
                    throw new Exception("Alias này đã tồn tại");
                }
                if (string.IsNullOrWhiteSpace(model.Alias))
                {
                    var i = 0;
                    do
                    {
                        model.Alias = (i == 0
                                           ? StringHelpers.MakeSEOTitle(model.Title)
                                           : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                        i++;
                    } while (_articleService.CheckExistAlias(model.Alias, model.Id));
                }
                var article = _articleService.GetById(model.Id);
                if (article == null) return RedirectToAction("List");

                article.Title = model.Title;
                article.Alias = model.Alias;

                article.Id = model.Id;
                article.Title = model.Title;
                article.Alias = model.Alias;
                article.Quote = model.Quote;
                article.Body = Server.HtmlDecode(model.Body);
                article.ModifiedDate = DateTime.UtcNow ;
                article.ModifiedUser = "";
                article.PublishedDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.PublishedDate);
                article.ExpiredDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.ExpiredDate);
                article.IsFeature = model.IsFeature;
                article.IsPublished = model.IsPublished;
                article.MetaKeywords = model.MetaKeywords;
                article.MetaDescription = model.MetaDescription;
                article.MetaTitle = model.MetaTitle;
                article.ModifiedUser = WorkContext.UserLoginInfo.UserName;

                _articleService.Update(article);
                SuccessNotification("Update article '" + model.Title + "' successful");
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var article = _articleService.GetById(id);

                _articleService.Delete(id);

                SuccessNotification("Delete article '" + article.Title + "' successful");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}
