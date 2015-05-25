using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace pCMS.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public RegisterModel()
        {
            //BusinessType = new int[0];
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

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Activation Type")]
        public string ActivationType { get; set; }

        public List<SelectListItem> ActivationTypes { get; set; }

        [Display(Name = "Resale #")]
        public string Resale { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Business Description")]
        public string BusinessDescription { get; set; }

        [Display(Name = "Taxpaper ID #")]
        public string TaxpaperId { get; set; }
        /*
        [Display(Name = "Company Name")]
        [Required]
        public string CompanyName { get; set; }

        public string DBA { get; set; }

        [Display(Name = "Applicant First Name")]
        [Required]
        public string ApplicantFirstName { get; set; }

        [Display(Name = "Applicant Last Name")]
        [Required]
        public string ApplicantLastName { get; set; }

        [Display(Name = "Company Address 1")]
        [Required]
        public string CompanyAddress1 { get; set; }

        [Display(Name = "Company Address 2")]
        public string CompanyAddress2 { get; set; }

        [Display(Name = "Company City")]
        [Required]
        public string CompanyCity { get; set; }

        [Display(Name = "State/Province")]
        [Required]
        public string StateProvince { get; set; }

        [Display(Name = "Zip/Postal Code")]
        [Required]
        public string ZipCode { get; set; }

        [Display(Name = "Fax Number")]
        public string FaxNumber { get; set; }

        [Display(Name = "Additional e-mail addresses:")]
        [Required]
        public string AdditionalEmail{ get; set; }

        [Display(Name = "Federal ID")]
        [Required]
        public string FederalID  { get; set; }

        [Display(Name = "Company Type")]
        [Required]
        public int CompanyType { get; set; }

        [Display(Name = "Business Type")]
        [Required]
        public int[] BusinessType { get; set; }

        [Display(Name = "How do you plan to purchase")]
        [Required]
        public int PurcharseType { get; set; }

        [Display(Name = "Please enter sales representative name if applicable")]
//        [Required]
        public string SalesRepresentativeName { get; set; }

        [Display(Name = "Would you like someone to contact you about our products and pricing?")]
        [Required]
        public bool ContactYou { get; set; }

        public List<SelectListItem> CompanyTypes { get; set; }
        public List<SelectListItem> BusinessTypes { get; set; }
        public List<SelectListItem> PurcharseTypes { get; set; }
        public List<SelectListItem> StateProvinces { get; set; }
         * */

    }

    public class ForgotPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class ConfirmResetPasswordModel
    {
        public string Message { get; set; }
    }
}
