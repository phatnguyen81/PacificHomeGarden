using System;

namespace pCMS.Core.Domain
{
    public class PCmsUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public string TimeZoneId { get; set; }
        public string ActivationType { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }

        public string Resale { get; set; }

        public string BusinessName { get; set; }

        public string Title { get; set; }

        public string BusinessDescription { get; set; }

        public string TaxpaperId { get; set; }

        /*
        public string CompanyName { get; set; }
        public string DBA { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyCity { get; set; }
        public string StateProvince { get; set; }
        public string ZipCode { get; set; }
        public string FaxNumber { get; set; }
        public string AdditionalEmail { get; set; }
        public string FederalID { get; set; }
        public int CompanyType { get; set; }
        public List<int> BusinessType { get; set; }
        public int PurcharseType { get; set; }
        public string SalesRepresentativeName { get; set; }
        public bool ContactYou { get; set; }
         */
    }
}
