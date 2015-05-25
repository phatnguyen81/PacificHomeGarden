using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core.Utils;
using pCMS.Framework.Controllers;
using pCMS.Core;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class ChannelController : BaseAdminController
    {
        private readonly IChannelService _channelService;

        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Albums(GridCommand command)
        {
            var channels = _channelService.SearchChannels(null, true, command.Page - 1, command.PageSize);
            var model = new GridModel<AlbumModel>
            {
                Data = channels.Select(
                    q => new AlbumModel()
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Alias = q.Alias,
                    }).ToList(),
                Total = channels.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }

        public ActionResult List(ChannelListModel model)
        {
            var channels = _channelService.SearchChannels(model.Keywords, true, 0, 20);
            model.Channels = new GridModel<ChannelModel>
                                 {
                                     Data = channels.Select(
                                         q => new ChannelModel
                                                  {
                                                      Id = q.Id,
                                                      Title = q.Title,
                                                      Alias = q.Alias
                                                  }).ToList(),
                                     Total = channels.TotalCount
                                 };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new ChannelItemModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ChannelItemModel model, bool continueEditing)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(model.Alias) && _channelService.CheckExistAlias(model.Alias))
                {
                    throw new Exception("Alias existed");
                }
                var i = 0;
                do
                {
                    model.Alias = (i == 0 ? StringHelpers.MakeSEOTitle(model.Title) : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                    i++;
                } while (_channelService.CheckExistAlias(model.Alias));
                    

                var channel = new Channel
                                    {
                                        Id = Guid.NewGuid(), 
                                        Title = model.Title, 
                                        Alias = model.Alias,
                                        MetaKeywords = model.MetaKeywords,
                                        MetaDescription = model.MetaDescription,
                                        MetaTitle = model.MetaTitle
                                    };
                _channelService.Add(channel);
                //_channelService.SaveChanges();
                SuccessNotification("Add channel '" + model.Title + "' successful");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = channel.Id })
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
            var channel = _channelService.GetById(id);
            if (channel == null) return RedirectToAction("List");

            var model = new ChannelItemModel()
                            {
                                Id = channel.Id,
                                Title = channel.Title,
                                Alias = channel.Alias,
                                MetaKeywords = channel.MetaKeywords,
                                MetaDescription = channel.MetaDescription,
                                MetaTitle = channel.MetaTitle
                            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(ChannelItemModel model, bool continueEditing)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Alias) && _channelService.CheckExistAlias(model.Alias, model.Id))
                {
                    throw new Exception("Alias existed");
                }
                var i = 0;
                do
                {
                    model.Alias = (i == 0 ? StringHelpers.MakeSEOTitle(model.Title) : StringHelpers.MakeSEOTitle(model.Title) + "-" + i);
                    i++;
                } while (_channelService.CheckExistAlias(model.Alias, model.Id));

                var channel = _channelService.GetById(model.Id);
                if (channel == null) return RedirectToAction("List");

                channel.Title = model.Title;
                channel.Alias = model.Alias;
                channel.MetaKeywords = model.MetaKeywords;
                channel.MetaDescription = model.MetaDescription;
                channel.MetaTitle = model.MetaTitle;

                _channelService.SaveChanges();

                SuccessNotification("Update channel '" + model.Title + "' successful");
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
                    var channel = _channelService.GetById(id);

                    _channelService.Delete(id);

                    SuccessNotification("Delete channel '" + channel.Title + "' successful");
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    ErrorNotification(ex.GetBaseException().Message, false);
                }
                return RedirectToAction("Edit", new {id});
        }
    }
}
