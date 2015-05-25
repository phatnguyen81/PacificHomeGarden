using System;
using System.Linq;
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
    public class PollController : BaseAdminController
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService;
        }

        #region Ajax
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult PollAnswers(Guid id, GridCommand command)
        {
            var poll = _pollService.GetById(id);
            if (poll == null)
                throw new ArgumentException("No poll found with the specified id", "pollId");

            var answers = poll.PollAnswers.Select(x => new PollAnswerModel()
                                                           {
                                                               Id = x.Id,
                                                               AnswerTitle = x.Title,
                                                               NumberOfVote = x.NumberOfVote,
                                                               DisplayOrder = x.DisplayOrder
                                                           }).ForCommand(command);

            var model = new GridModel<PollAnswerModel>
            {
                Data = answers.PagedForCommand(command),
                Total = answers.Count()
            };
            return new JsonResult
            {
                Data = model
            };
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult PollAnswerUpdate(PollAnswerModel model, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult { Data = "error" };
            }
            var pollAnswer = _pollService.GetAnswerById(model.Id);
            pollAnswer.Title = model.AnswerTitle;
            pollAnswer.DisplayOrder = model.DisplayOrder;
            _pollService.SaveChanges();

            return PollAnswers(pollAnswer.PollId, command);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult PollAnswerAdd(Guid id, PollAnswerModel model, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult { Data = "error" };
            }
            var poll = _pollService.GetById(id);
            if (poll == null)
                throw new ArgumentException("No poll found with the specified id", "pollId");

            poll.PollAnswers.Add(new PollAnswer
                                        {
                                            Id = Guid.NewGuid(),
                                            Title = model.AnswerTitle,
                                            DisplayOrder = model.DisplayOrder
                                        });
            _pollService.SaveChanges();

            return PollAnswers(poll.Id, command);
        }


        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult PollAnswerDelete(Guid id, GridCommand command)
        {
            var pollAnswer = _pollService.GetAnswerById(id);
            var pollId = pollAnswer.PollId;
            _pollService.DeleteAnswer(pollAnswer);
            _pollService.SaveChanges();

            return PollAnswers(pollId, command);
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Polls(GridCommand command)
        {
            var polls = _pollService.SearchPoll(null, true, command.Page - 1, command.PageSize);
            var model = new GridModel<PollModel>
            {
                Data = polls.Select(
                    q => new PollModel
                    {
                        Id = q.Id,
                        Title = q.Title,
                        IsPublished = q.IsPublished,
                        NumberOfAnswer = q.PollAnswers.Count,
                        StartDate = q.StartDate,
                        EndDate = q.EndDate
                    }).ToList(),
                Total = polls.TotalCount

            };

            return new JsonResult
            {
                Data = model
            };
        }
        #endregion

        public ActionResult List(PollListModel model)
        {
            var polls = _pollService.SearchPoll(null, true, 0, 20);
            model.Polls = new GridModel<PollModel>
                              {
                                  Data = polls.Select(
                                      q => new PollModel
                                               {
                                                   Id = q.Id,
                                                   Title = q.Title,
                                                   IsPublished = q.IsPublished,
                                                   NumberOfAnswer = q.PollAnswers.Count,
                                                   StartDate = q.StartDate,
                                                   EndDate = q.EndDate
                                               }).ToList(),
                                  Total = polls.TotalCount

                              };
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new PollItemModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(PollItemModel model, bool continueEditing)
        {
                 try
                 {
                     if (model.StartDate != null && model.EndDate != null && model.StartDate > model.EndDate)
                         throw new Exception("Ngày kết thúc phải lớn hơn ngày bắt đầu");

                     var poll = new Poll
                                    {
                                        Id= Guid.NewGuid(),
                                        Title = model.Title,
                                        IsPublished = model.IsPublished,
                                        StartDate = model.StartDate,
                                        EndDate = model.EndDate
                                    };
                     _pollService.Add(poll);
                     _pollService.SaveChanges();
                     SuccessNotification("Thêm mới bình chọn '" + model.Title + "' thành công");
                     return continueEditing
                                ? RedirectToAction("Edit", new { id = poll.Id })
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
            var poll = _pollService.GetById(id);
            if (poll == null) return RedirectToAction("List");

            var model = new PollItemModel()
            {
                Id = poll.Id,
                Title = poll.Title,
                IsPublished = poll.IsPublished,
                StartDate = poll.StartDate,
                EndDate = poll.EndDate
            };
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(PollItemModel model, bool continueEditing)
        {
            try
            {
                if (model.StartDate != null && model.EndDate != null && model.StartDate > model.EndDate)
                    throw new Exception("Ngày kết thúc phải lớn hơn ngày bắt đầu");
                var poll = _pollService.GetById(model.Id);
                if (poll == null) return RedirectToAction("List");

                poll.Title = model.Title;
                poll.IsPublished = model.IsPublished;
                poll.StartDate = model.StartDate;
                poll.EndDate = model.EndDate;

                _pollService.SaveChanges();

                SuccessNotification("Cập nhật bình chọn '" + model.Title + "' thành công");
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
                var poll = _pollService.GetById(id);

                _pollService.Delete(id);
                _pollService.SaveChanges();

                SuccessNotification("Xóa bình chọn '" + poll.Title + "' thành công");
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
