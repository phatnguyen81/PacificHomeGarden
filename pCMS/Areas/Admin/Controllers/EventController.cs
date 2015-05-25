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
    public class EventController : BaseAdminController
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Events(GridCommand command)
        {
            var events = _eventService.SearchEvents(null, true, command.Page - 1, command.PageSize);
            var model = new GridModel<EventModel>
            {
                Data = events.Select(
                         q => new EventModel()
                         {
                             Id = q.Id,
                             Title = q.Title,
                             Description = q.Description,
                             DateBegin = q.DateBegin,
                             DateEnd = q.DateBegin,
                             PublishedDate = DateTimeHelpers.ConvertUtcToUserTimeZone(q.PublishedDate),
                             ExpiredDate = DateTimeHelpers.ConvertUtcToUserTimeZone(q.ExpiredDate),
                             Location = q.Location,
                             City = q.City,
                             Booth = q.Booth,
                             IsPublished = q.IsPublished
                         }).ToList(),
                Total = events.TotalCount
            };

            return new JsonResult
            {
                Data = model
            };
        }
        public ActionResult List(EventListModel model)
        {
            var events = _eventService.SearchEvents(null, true, 0, 20);
            model.Events = new GridModel<EventModel>
                               {
                                   Data = events.Select(
                                       q => new EventModel()
                                                {
                                                    Id = q.Id,
                                                    Title = q.Title,
                                                    Description = q.Description,
                                                    DateBegin = q.DateBegin,
                                                    DateEnd = q.DateBegin,
                                                    PublishedDate =
                                                        DateTimeHelpers.ConvertUtcToUserTimeZone(q.PublishedDate),
                                                    ExpiredDate =
                                                        DateTimeHelpers.ConvertUtcToUserTimeZone(q.ExpiredDate),
                                                    Location = q.Location,
                                                    City = q.City,
                                                    Booth = q.Booth,
                                                    IsPublished = q.IsPublished
                                                }).ToList(),
                                   Total = events.TotalCount
                               };
            return View(model);
                
        }

        public ActionResult Create()
        {

            var model = new EventItemModel();
            return View(model);
        }
        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(EventItemModel model, bool continueEditing)
        {
            try
            {

                var eventt = new Event
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    IsPublished = model.IsPublished,
                    Location = model.Location,
                    LocationLink = model.LocationLink,
                    City = model.City,
                    Booth = model.Booth,
                    DateBegin = model.DateBegin.Date,
                    DateEnd = model.DateEnd.Date,
                    PublishedDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.PublishedDate),
                    ExpiredDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.ExpiredDate)
                };
                _eventService.Add(eventt);
                _eventService.SaveChanges();
                SuccessNotification("Add new event '" + model.Title + "' successful");
                return continueEditing
                            ? RedirectToAction("Edit", new { id = eventt.Id })
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
            var eventt = _eventService.GetById(id);
            var model = new EventItemModel
            {
                Title = eventt.Title,
                Id = eventt.Id,
                Description = eventt.Description,
                IsPublished = eventt.IsPublished,
                Location = eventt.Location,
                LocationLink = eventt.LocationLink,
                City = eventt.City,
                Booth = eventt.Booth,
                DateBegin = eventt.DateBegin,
                DateEnd = eventt.DateEnd,
                PublishedDate = DateTimeHelpers.ConvertUtcToUserTimeZone(eventt.PublishedDate),
                ExpiredDate = DateTimeHelpers.ConvertUtcToUserTimeZone(eventt.ExpiredDate)
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(EventItemModel model, bool continueEditing)
        {
            try
            {
                var eventt = _eventService.GetById(model.Id);
                if (eventt == null) return RedirectToAction("List");

                eventt.Title = model.Title;
                eventt.Description = model.Description;
                eventt.IsPublished = model.IsPublished;
                eventt.Location = model.Location;
                eventt.LocationLink = model.LocationLink;
                eventt.City = model.City;
                eventt.Booth = model.Booth;
                eventt.DateBegin = model.DateBegin;
                eventt.DateEnd = model.DateEnd;
                eventt.PublishedDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.PublishedDate);
                eventt.ExpiredDate = DateTimeHelpers.ConvertUserTimeZoneToUtc(model.ExpiredDate);
                _eventService.SaveChanges();
                SuccessNotification("Update event '" + model.Title + "' successful");
                return continueEditing
                                ? RedirectToAction("Edit", new { id = eventt.Id })
                                : RedirectToAction("List");
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
                var eventt = _eventService.GetById(id);

                _eventService.Delete(id);
                _eventService.SaveChanges();

                SuccessNotification("Delete event '" + eventt.Title + "' successful");
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
