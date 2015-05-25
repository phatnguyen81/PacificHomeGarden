using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Telerik.Web.Mvc;
using pCMS.Core;
using pCMS.Core.Domain;

namespace pCMS.Models
{
    public class ContactModel
    {
        [Display(Name = "Resale #")]
        public string Resale { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "E-mail address")]
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Business Description")]
        public string BusinessDescription { get; set; }

        [Display(Name = "Taxpaper ID #")]
        public string TaxpaperId{ get; set; }

        [Required]
        [Display(Name = "Enter your message")]
        public string Message { get; set; }

        public string SecurityCode { get; set; }
    }

    public class ChildCategoryListModel
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
    }

    public class CategoryDetailModel
    {
        public string Alias { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public List<ChildCategoryListModel> ChildCategories { get; set; } 
    }

    public class SearchModel
    {
        public SearchModel()
        {
        }

        public string Keywords { get; set; }
        public GridModel<SearchItem> SearchResults { get; set; }
        public class SearchItem
        {
            public Guid Id { get; set; }
            public DocumentType Type { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Keywords { get; set; }
            public Guid ParentId { get; set; }
        } 
    }

    
}