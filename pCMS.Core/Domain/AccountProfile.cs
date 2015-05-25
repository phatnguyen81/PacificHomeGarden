using System;
using System.Collections.Generic;
using System.Web.Profile;

namespace pCMS.Core.Domain
{
    public class AccountProfile : ProfileBase
    {
        public static AccountProfile GetUserProfile(string username)
        {
            return Create(username) as AccountProfile;
        }

        [SettingsAllowAnonymous(false)]
        public string FullName
        {
            get { return this["FullName"] as string; }
            set { this["FullName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string TimeZoneId
        {
            get { return this["TimeZoneId"] as string; }
            set { this["TimeZoneId"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string PhoneNumber
        {
            get { return this["PhoneNumber"] as string; }
            set { this["PhoneNumber"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string Address
        {
            get { return this["Address"] as string; }
            set { this["Address"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string ActivationType
        {
            get { return this["ActivationType"] as string; }
            set { this["ActivationType"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public Guid TokenResetPassword
        {
            get 
            { 
                try
                {
                    return new Guid(this["TokenResetPassword"].ToString());
                }
                catch
                {
                    return Guid.Empty;
                }
            }
            set { this["TokenResetPassword"] = value; }
        }
        #region RegiterForm
        [SettingsAllowAnonymous(false)]
        public string Resale
        {
            get { return this["Resale"] as string; }
            set { this["Resale"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string BusinessName
        {
            get { return this["BusinessName"] as string; }
            set { this["BusinessName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string Title
        {
            get { return this["Title"] as string; }
            set { this["Title"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string BusinessDescription
        {
            get { return this["BusinessDescription"] as string; }
            set { this["BusinessDescription"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string TaxpaperId
        {
            get { return this["TaxpaperId"] as string; }
            set { this["TaxpaperId"] = value; }
        }
        #endregion


        /*
        #region RegiterForm

        [SettingsAllowAnonymous(false)]
        public string CompanyName
        {
            get { return this["CompanyName"] as string; }
            set { this["CompanyName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string DBA
        {
            get { return this["DBA"] as string; }
            set { this["DBA"] = value; }
        }

        public string ApplicantFirstName
        {
            get { return this["ApplicantFirstName"] as string; }
            set { this["ApplicantFirstName"] = value; }
        }

        public string ApplicantLastName
        {
            get { return this["ApplicantLastName"] as string; }
            set { this["ApplicantLastName"] = value; }
        }

        public string CompanyAddress1
        {
            get { return this["CompanyAddress1"] as string; }
            set { this["CompanyAddress1"] = value; }
        }

        public string CompanyAddress2
        {
            get { return this["CompanyAddress2"] as string; }
            set { this["CompanyAddress2"] = value; }
        }

        public string CompanyCity
        {
            get { return this["CompanyCity"] as string; }
            set { this["CompanyCity"] = value; }
        }

        public string StateProvince 
        {
            get { return this["StateProvince"] as string; }
            set { this["StateProvince"] = value; }
        }

        public string ZipCode
        {
            get { return this["ZipCode"] as string; }
            set { this["ZipCode"] = value; }
        }

        public string FaxNumber
        {
            get { return this["FaxNumber"] as string; }
            set { this["FaxNumber"] = value; }
        }

        public string AdditionalEmail
        {
            get { return this["AdditionalEmail"] as string; }
            set { this["AdditionalEmail"] = value; }
        }

        public string FederalID 
        {
            get { return this["FederalID"] as string; }
            set { this["FederalID"] = value; }
        }

        public int CompanyType
        {
            get
            {
                int type;
                int.TryParse(this["CompanyType"].ToString(),out type);
                return type;
            }
            set { this["CompanyType"] = value; }
        }

        public List<int> BusinessType
        {
            get
            {
                var businessType = new List<int>();
                //foreach (var s in this["BusinessType"].ToString().Split(','))
                //{
                //    var type = -1;
                //    int.TryParse(s, out type);
                //    if (type >= 0)
                //    {
                //        businessType.Add(type);
                //    }
                //}
                if (this["BusinessType"] == null) return new List<int>();
                return (List<int>) this["BusinessType"];
                //return businessType;
            }
            set { this["BusinessType"] = value; }
        }
        public int PurcharseType
        {
            get
            {
                int type;
                int.TryParse(this["PurcharseType"].ToString(), out type);
                return type;
            }
            set { this["PurcharseType"] = value; }
        }

        public string SalesRepresentativeName
        {
            get { return this["SalesRepresentativeName"] as string; }
            set { this["SalesRepresentativeName"] = value; }
        }

        public bool ContactYou
        {
            get { return Convert.ToBoolean(this["ContactYou"]); }
            set { this["ContactYou"] = value; }
        }
        #endregion
         */
    }
}