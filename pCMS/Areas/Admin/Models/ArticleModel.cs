using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using pCMS.Core.Utils;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class ArticleListModel
    {
        public ArticleListModel()
        {
            Published = string.Empty;
            Featured = string.Empty;
            PublishedOptions = new List<SelectListItem>
                                   {
                                       new SelectListItem{Text = "All" , Value = "All"},
                                       new SelectListItem{Text = "Published" , Value = "True"},
                                       new SelectListItem{Text = "Unpublish" , Value = "False"}
                                   };
            FeaturedOptions = new List<SelectListItem>
                                   {
                                       new SelectListItem{Text = "All" , Value = "All"},
                                       new SelectListItem{Text = "Featured" , Value = "True"},
                                       new SelectListItem{Text = "UnFeature" , Value = "False"}
                                   };
        }
        public string Keywords { get; set; }
        public string Published { get; set; }
        public string Featured { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> PublishedOptions { get; set; }
        public List<SelectListItem> FeaturedOptions { get; set; }
        public GridModel<ArticleModel> Articles { get; set; }
    }
    public class ArticleModel
    {
        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Quote")]
        public string Quote { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Body")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.CreatedUser")]
        public string CreatedUser { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.ModifiedUser")]
        public string ModifiedUser { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.DeletedUser")]
        public string DeletedUser { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.ExpiredDate")]
        public DateTime? ExpiredDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.DeletedDate")]
        public DateTime? DeletedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.IsDeleted")]
        public bool IsDeleted { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.IsFeature")]
        public bool IsFeature { get; set; }
    }
    
    public class ArticleItemModel
    {


        public ArticleItemModel()
        {
            IsFeature = false;
            IsDeleted = false;
            IsPublished = false;
            PublishedDate = DateTimeHelpers.Now;
        }



        public Guid Id { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Title")]
        [Required]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Alias")]
        public string Alias { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Quote")]
        public string Quote { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.Body")]
        [UIHint("RichEditor")]
        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Body { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.IsPublished")]
        public bool IsPublished { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.CreatedUser")]
        public string CreatedUser { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.ModifiedUser")]
        public string ModifiedUser { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.DeletedUser")]
        public string DeletedUser { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        [Required]
        [UIHint("DateTime")]
        [ResourceDisplayName("Admin.Content.Article.Fields.PublishedDate")]
        public DateTime PublishedDate { get; set; }

        [UIHint("DateTimeNullable")]
        [ResourceDisplayName("Admin.Content.Article.Fields.ExpiredDate")]
        public DateTime? ExpiredDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.DeletedDate")]
        public DateTime? DeletedDate { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.IsDeleted")]
        public bool IsDeleted { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.IsFeature")]
        public bool IsFeature { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [ResourceDisplayName("Admin.Content.Article.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        public class ArticleChannelModel
        {
            //public string Id { get; set; }

            public Guid ChannelId { get; set; }
            public Guid ArticleId { get; set; }

            [ResourceDisplayName("Admin.Content.ArticleChannel.Fields.IsFeatured")]
            public bool ArticleChannelIsFeatured { get; set; }

            [AllowHtml]
            [ResourceDisplayName("Admin.Content.ArticleChannel.Fields.Title")]
            [UIHint("ChannelArticle")]
            public string ChannelTitle { get; set; }

        }


    }

}