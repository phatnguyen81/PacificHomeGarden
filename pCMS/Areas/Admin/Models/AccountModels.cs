using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.Web.Mvc;
using pCMS.Framework;

namespace pCMS.Admin.Models
{
    public class AccountModel
    {
        public AccountModel()
        {
            
        }


        [ResourceDisplayName("Admin.Accounts.Fields.UserName")]
        public string UserName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.FullName")]
        public string FullName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Email")]
        public string Email { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.IsLockedOut")]
        public bool IsLockedOut { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.CreationDate")]
        public DateTime CreationDate { get; set; }
    }

    public class AccountListModel
    {
        public AccountListModel()
        {
            SearchTypes = new List<SelectListItem>
                              {
                                  new SelectListItem{Text = "User Name",Value = "NAME"},
                                  new SelectListItem{Text = "Email",Value = "EMAIL"}
                              };
            SearchType = "NAME";
        }
        public string SearchType { get; set; }
        public List<SelectListItem> SearchTypes { get; set; }
        public string Keyword { get; set; }
        public GridModel<AccountModel> Accounts { get; set; }

    }

    public class RoleListModel
    {
        [ResourceDisplayName("Admin.Roles.Fields.RoleName")]
        public string RoleName { get; set; }
    }

    public class RoleCreateOrUpdateModel
    {
        public bool IsEdit { get; set; }

        [ResourceDisplayName("Admin.Roles.Fields.RoleName")]
        [Required]
        public string RoleName { get; set; } 
    }

    public class AccountCreateModel
    {

        public AccountCreateModel()
        {
            IsApproved = true;
           
        }

        [ResourceDisplayName("Admin.Accounts.Fields.UserName")]
        [Required]
        public string UserName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Password")]
        [Required]
        public string Password { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Email")]
        public string Email { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.FullName")]
        [Required]
        public string FullName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Address")]
        public string Address { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.ActivationType")]
        public string ActivationType { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.IsApproved")]
        public bool IsApproved { get; set; }

    }

    public class AccountCreateOrUpdateModel
    {
        public AccountCreateOrUpdateModel()
        {
            ProviderUserKey = Guid.Empty;
            AllRoles = Roles.GetAllRoles();
            SelectedRoles = new List<string>();


            var tzCollection = TimeZoneInfo.GetSystemTimeZones();
            AllTimeZones = new List<SelectListItem>();
            foreach (var timeZone in tzCollection)
            {
                AllTimeZones.Add(new SelectListItem{Value = timeZone.Id, Text = timeZone.DisplayName});
            }
            ActivationTypes = new List<SelectListItem>
                                  {
                                      new SelectListItem{Text = "By phone",Value = "PHONE"},
                                      new SelectListItem{Text = "By email",Value = "EMAIL"}
                                  };
            /*
            CompanyTypes = new List<SelectListItem>
                                  {
                                      new SelectListItem{Text = "Corporation",Value = "1"},
                                      new SelectListItem{Text = "Non-Profit",Value = "2"},
                                      new SelectListItem{Text = "Partnership",Value = "3"},
                                      new SelectListItem{Text = "Sole Proprietor",Value = "4"}
                                  };
            BusinessTypes = new List<SelectListItem>
                                  {
                                      new SelectListItem{Text = "Wholesale",Value = "1"},
                                      new SelectListItem{Text = "Retail",Value = "2"},
                                      new SelectListItem{Text = "Manufacturing",Value = "3"}
                                  };
            PurcharseTypes = new List<SelectListItem>
                                  {
                                      new SelectListItem{Text = "Open Terms",Value = "1"},
                                      new SelectListItem{Text = "Credit Card",Value = "2"}
                                  };
            StateProvinces = new List<SelectListItem>
                                 {
                                     new SelectListItem {Text = "Alaska", Value = "Alaska"},
                                     new SelectListItem {Text = "Alabama", Value = "Alabama"},
                                     new SelectListItem {Text = "Arkansas", Value = "Arkansas"},
                                     new SelectListItem {Text = "American Samoa", Value = "American Samoa"},
                                     new SelectListItem {Text = "Arizona", Value = "Arizona"},
                                     new SelectListItem {Text = "California", Value = "California"},
                                     new SelectListItem {Text = "Colorado", Value = "Colorado"},
                                     new SelectListItem {Text = "Connecticut", Value = "Connecticut"},
                                     new SelectListItem {Text = "District of Columbia", Value = "District of Columbia"},
                                     new SelectListItem {Text = "Delaware", Value = "Delaware"},
                                     new SelectListItem {Text = "Florida", Value = "Florida"},
                                     new SelectListItem
                                         {
                                             Text = "Federated States of Micronesia",
                                             Value = "Federated States of Micronesia"
                                         },
                                     new SelectListItem {Text = "Georgia", Value = "Georgia"},
                                     new SelectListItem {Text = "Guam", Value = "Guam"},
                                     new SelectListItem {Text = "Hawaii", Value = "Hawaii"},
                                     new SelectListItem {Text = "Iowa", Value = "Iowa"},
                                     new SelectListItem {Text = "Idaho", Value = "Idaho"},
                                     new SelectListItem {Text = "Illinois", Value = "Illinois"},
                                     new SelectListItem {Text = "Indiana", Value = "Indiana"},
                                     new SelectListItem {Text = "Kansas", Value = "Kansas"},
                                     new SelectListItem {Text = "Kentucky", Value = "Kentucky"},
                                     new SelectListItem {Text = "Louisiana", Value = "Louisiana"},
                                     new SelectListItem {Text = "Massachusetts", Value = "Massachusetts"},
                                     new SelectListItem {Text = "Maine", Value = "Maine"},
                                     new SelectListItem {Text = "Maryland", Value = "Maryland"},
                                     new SelectListItem {Text = "Marshall Islands", Value = "Marshall Islands"},
                                     new SelectListItem {Text = "Michigan", Value = "Michigan"},
                                     new SelectListItem {Text = "Minnesota", Value = "Minnesota"},
                                     new SelectListItem {Text = "Missouri", Value = "Missouri"},
                                     new SelectListItem
                                         {Text = "Northern Mariana Islands", Value = "Northern Mariana Islands"},
                                     new SelectListItem {Text = "Mississippi", Value = "Mississippi"},
                                     new SelectListItem {Text = "Montana", Value = "Montana"},
                                     new SelectListItem {Text = "North Carolina", Value = "North Carolina"},
                                     new SelectListItem {Text = "North Dakota", Value = "North Dakota"},
                                     new SelectListItem {Text = "Nebraska", Value = "Nebraska"},
                                     new SelectListItem {Text = "New Hampshire", Value = "New Hampshire"},
                                     new SelectListItem {Text = "New Jersey", Value = "New Jersey"},
                                     new SelectListItem {Text = "New Mexico", Value = "New Mexico"},
                                     new SelectListItem {Text = "Nevada", Value = "Nevada"},
                                     new SelectListItem {Text = "New York", Value = "New York"},
                                     new SelectListItem {Text = "Ohio", Value = "Ohio"},
                                     new SelectListItem {Text = "Oklahoma", Value = "Oklahoma"},
                                     new SelectListItem {Text = "Oregon", Value = "Oregon"},
                                     new SelectListItem {Text = "Pennsylvania", Value = "Pennsylvania"},
                                     new SelectListItem {Text = "Puerto Rico", Value = "Puerto Rico"},
                                     new SelectListItem {Text = "Palau", Value = "Palau"},
                                     new SelectListItem {Text = "Rhode Island", Value = "Rhode Island"},
                                     new SelectListItem {Text = "South Carolina", Value = "South Carolina"},
                                     new SelectListItem {Text = "South Dakota", Value = "South Dakota"},
                                     new SelectListItem {Text = "Tennessee", Value = "Tennessee"},
                                     new SelectListItem {Text = "Texas", Value = "Texas"},
                                     new SelectListItem {Text = "Utah", Value = "Utah"},
                                     new SelectListItem {Text = "Virgin Islands", Value = "Virgin Islands"},
                                     new SelectListItem {Text = "Vermont", Value = "Vermont"},
                                     new SelectListItem {Text = "Virginia", Value = "Virginia"},
                                     new SelectListItem {Text = "Washington", Value = "Washington"},
                                     new SelectListItem {Text = "Wisconsin", Value = "Wisconsin"},
                                     new SelectListItem {Text = "West Virginia", Value = "West Virginia"},
                                     new SelectListItem {Text = "Wyoming", Value = "Wyoming"}

                                 };
             */

        }

        public Guid ProviderUserKey { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.UserName")]
        [Required]
        public string UserName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Password")]
        [Required]
        public string Password { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.NewPassword")]
        public string NewPassword { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Email")]
        public string Email { get; set; }
        
        [ResourceDisplayName("Admin.Accounts.Fields.IsLockedOut")]
        public bool IsLockedOut { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.FullName")]
        [Required]
        public string FullName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.TimeZone")]
        public string TimeZoneId { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Address")]
        public string Address { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.ActivationType")]
        public string ActivationType { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.CreationDate")]
        public DateTime CreationDate { get; set; }

        public List<SelectListItem> AllTimeZones { get; set; }
        public IEnumerable<string> AllRoles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }

        public List<SelectListItem> ActivationTypes { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Resale")]
        public string Resale { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.BusinessName")]
        public string BusinessName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.Title")]
        public string Title { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.BusinessDescription")]
        public string BusinessDescription { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.TaxpaperId")]
        public string TaxpaperId { get; set; }
        /*
        [Display(Name = "Company Name")]
        //[Required]
        [ResourceDisplayName("Admin.Accounts.Fields.CompanyName")]
        public string CompanyName { get; set; }

        public string DBA { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.ApplicantFirstName")]
        //[Required]
        public string ApplicantFirstName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.ApplicantLastName")]
        //[Required]
        public string ApplicantLastName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.CompanyAddress1")]
        //[Required]
        public string CompanyAddress1 { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.CompanyAddress2")]
        public string CompanyAddress2 { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.CompanyCity")]
        //[Required]
        public string CompanyCity { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.StateProvince")]
        //[Required]
        public string StateProvince { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.ZipCode")]
        //[Required]
        public string ZipCode { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.FaxNumber")]
        public string FaxNumber { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.AdditionalEmail")]
        //[Required]
        public string AdditionalEmail { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.FederalID")]
        //[Required]
        public string FederalID { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.CompanyType")]
        //[Required]
        public int CompanyType { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.BusinessType")]
        //[Required]
        public int[] BusinessType { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.PurcharseType")]
        //[Required]
        public int PurcharseType { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.SalesRepresentativeName")]
        //[Required]
        public string SalesRepresentativeName { get; set; }

        [ResourceDisplayName("Admin.Accounts.Fields.ContactYou")]
        //[Required]
        public bool ContactYou { get; set; }

        public List<SelectListItem> CompanyTypes { get; set; }
        public List<SelectListItem> BusinessTypes { get; set; }
        public List<SelectListItem> PurcharseTypes { get; set; }
        public List<SelectListItem> StateProvinces { get; set; }
         */

    }
}