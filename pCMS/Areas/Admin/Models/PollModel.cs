using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class PollListModel
    {
        public GridModel<PollModel> Polls { get; set; }
    }
    public class PollModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.StartDate")]
        public DateTime? StartDate { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.EndDate")]
        public DateTime? EndDate { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.NumberOfAnswer")]
        public int NumberOfAnswer { get; set; }
    }
    public class PollItemModel
    {
        public PollItemModel()
        {
            IsPublished = false;
        }

        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        [UIHint("DateNullable")]
        [ResourceDisplayName("Admin.Content.Poll.Fields.StartDate")]
        public DateTime? StartDate { get; set; }

        [UIHint("DateNullable")]
        [ResourceDisplayName("Admin.Content.Poll.Fields.EndDate")]
        public DateTime? EndDate { get; set; }
    }

    public class PollAnswerModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.AnswerTitle")]
        [AllowHtml]
        [Required]
        public string AnswerTitle { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.NumberOfVote")]
        public long NumberOfVote { get; set; }

        [ResourceDisplayName("Admin.Content.Poll.Fields.DisplayOrder")]
        [UIHint("DisplayOrder")]
        [Required]
        public int DisplayOrder { get; set; }
    }
}