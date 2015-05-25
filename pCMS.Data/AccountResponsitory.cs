using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Security;
using pCMS.Core;
using pCMS.Core.Domain;

namespace pCMS.Data
{
    public interface IUserRepository : IRepository<PCmsUser>
    {
        PCmsUser Find(string username);
        Guid CreateTokenResetPassword(string username);
        Guid GetTokenResetPassword(string username);
        string ResetPassowrd(string username);
        void ChangePassowrd(string username, string newpassword);
        void ResetTokenResetPassword(string username);
        List<PCmsUser> FindUserByEmail(string email,UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord);
        List<PCmsUser> FindUserByName(string name, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord);
    }


    public class UserRepository : IUserRepository
    {
        public void Dispose()
        {
            
        }

        public IQueryable<PCmsUser> All()
        {
            return (from MembershipUser user in Membership.GetAllUsers() select ConvertTopCmsUser(user)).AsQueryable();
        }

        public IQueryable<PCmsUser> Filter(Expression<Func<PCmsUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PCmsUser> Filter(Expression<Func<PCmsUser, bool>> filter, out int total, int index = 0, int size = 50)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Expression<Func<PCmsUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public PCmsUser Find(Expression<Func<PCmsUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(PCmsUser t)
        {
            MembershipCreateStatus createStatus;
            Membership.CreateUser(t.UserName, t.Password, t.Email, null, null, false, null, out createStatus);
            if(createStatus != MembershipCreateStatus.Success)
            {
                throw new Exception(ErrorCodeToString(createStatus));
            }
            try
            {
                var profile = AccountProfile.GetUserProfile(t.UserName);
                profile.PhoneNumber = t.PhoneNumber;
                //profile.Address = t.Address;
                profile.FullName = t.FullName;
                profile.ActivationType = t.ActivationType;
                profile.Resale = t.Resale;
                profile.BusinessName = t.BusinessName;
                profile.BusinessDescription = t.BusinessDescription;
                profile.TaxpaperId = t.TaxpaperId;
                profile.Title = t.Title;
                profile.Address = t.Address;
                /*
                profile.CompanyName = t.CompanyName;
                profile.DBA = t.DBA;
                profile.ApplicantFirstName = t.ApplicantFirstName;
                profile.ApplicantLastName = t.ApplicantLastName;
                profile.CompanyAddress1 = t.CompanyAddress1;
                profile.CompanyAddress2 = t.CompanyAddress2;
                profile.CompanyCity = t.CompanyCity;
                profile.StateProvince = t.StateProvince;
                profile.ZipCode = t.ZipCode;
                profile.FaxNumber = t.FaxNumber;
                profile.AdditionalEmail = t.AdditionalEmail;
                profile.FederalID = t.FederalID;
                profile.CompanyType = t.CompanyType;
                profile.BusinessType = t.BusinessType;
                profile.PurcharseType = t.PurcharseType;
                profile.SalesRepresentativeName = t.SalesRepresentativeName;
                profile.ContactYou = t.ContactYou;
                 */
                profile.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("User create successful but cannot create profile. Error : " + ex.GetBaseException().Message);
            }
        }

        public void Delete(PCmsUser t)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<PCmsUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Update(PCmsUser t)
        {
            var user = Membership.GetUser(t.UserName);
            if(user == null) throw new Exception("User no found");
            user.Email = t.Email;
            user.IsApproved = t.IsApproved;

            var profile = AccountProfile.GetUserProfile(t.UserName);
            profile.PhoneNumber = t.PhoneNumber;
            profile.Address = t.Address;
            profile.FullName = t.FullName;
            profile.ActivationType = t.ActivationType;

            profile.Resale = t.Resale;
            profile.BusinessName = t.BusinessName;
            profile.Title = t.Title;
            profile.BusinessDescription = t.BusinessDescription;
            profile.TaxpaperId = t.TaxpaperId;
            profile.Address = t.Address;

           
            profile.Save();
            Membership.UpdateUser(user);
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public PCmsUser Find(string username)
        {
            var user = Membership.GetUser(username);
            if(user == null) return null;
            return ConvertTopCmsUser(user);
        }

        public Guid CreateTokenResetPassword(string username)
        {
            var user = Membership.GetUser(username);
            if (user == null) return Guid.Empty;
            var profile = AccountProfile.GetUserProfile(username);
            var token = Guid.NewGuid();
            profile.TokenResetPassword = token;
            profile.Save();
            return token;
        }

        public Guid GetTokenResetPassword(string username)
        {
            var user = Membership.GetUser(username);
            if (user == null) return Guid.Empty;
            var profile = AccountProfile.GetUserProfile(username);
            return profile.TokenResetPassword;
        }

        public string ResetPassowrd(string username)
        {
            var user = Membership.GetUser(username);
            return user.ResetPassword();
        }

        public void ChangePassowrd(string username, string newpassword)
        {

            var user = Membership.GetUser(username);
            user.ChangePassword(user.ResetPassword(), newpassword);
        }

        public void ResetTokenResetPassword(string username)
        {
            var user = Membership.GetUser(username);
            if (user == null) return;
            var profile = AccountProfile.GetUserProfile(username);
            profile.TokenResetPassword = Guid.Empty;
            profile.Save();
        }

        public List<PCmsUser> FindUserByEmail(string email, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord)
        {
            var allUser = All().AsQueryable();
            if(!string.IsNullOrWhiteSpace(email))
            {
                allUser = allUser.Where(q => q.Email.Contains(email));
            }
            totalRecord = allUser.Count();
            switch (userSorting)
            {
                case UserSortingEnum.UserNameAsc:
                    allUser = allUser.OrderBy(q => q.UserName);
                    break;
                case UserSortingEnum.UserNameDesc:
                    allUser = allUser.OrderByDescending(q => q.UserName);
                    break;
                case UserSortingEnum.FullNameAsc:
                    allUser = allUser.OrderBy(q => q.FullName);
                    break;
                case UserSortingEnum.FullNameDesc:
                    allUser = allUser.OrderByDescending(q => q.FullName);
                    break;
                case UserSortingEnum.EmailAsc:
                    allUser = allUser.OrderBy(q => q.Email);
                    break;
                case UserSortingEnum.EmailDesc:
                    allUser = allUser.OrderByDescending(q => q.Email);
                    break;
                case UserSortingEnum.IsApprovedAsc:
                    allUser = allUser.OrderBy(q => q.IsApproved);
                    break;
                case UserSortingEnum.IsApprovedDesc:
                    allUser = allUser.OrderByDescending(q => q.IsApproved);
                    break;
                case UserSortingEnum.IsLockedOutAsc:
                    allUser = allUser.OrderBy(q => q.IsLockedOut);
                    break;
                case UserSortingEnum.IsLockedOutDesc:
                    allUser = allUser.OrderByDescending(q => q.IsLockedOut);
                    break;
                case UserSortingEnum.CreationDateAsc:
                    allUser = allUser.OrderBy(q => q.CreationDate);
                    break;
                case UserSortingEnum.CreationDateDesc:
                    allUser = allUser.OrderByDescending(q => q.CreationDate);
                    break;
            }
            return allUser.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            //if(string.IsNullOrWhiteSpace(email))
            //{
            //    return (from MembershipUser user in Membership.GetAllUsers(pageIndex, pageSize, out totalRecord) select ConvertTopCmsUser(user)).ToList();
            //}
            //return (from MembershipUser user in Membership.FindUsersByEmail("%" + email + "%", pageIndex, pageSize, out totalRecord) select ConvertTopCmsUser(user)).ToList();
        }

        public List<PCmsUser> FindUserByName(string name, UserSortingEnum userSorting, int pageIndex, int pageSize, out int totalRecord)
        {
            var allUser = All().AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                allUser = allUser.Where(q => q.UserName.Contains(name));
            }
            totalRecord = allUser.Count();
            switch (userSorting)
            {
                case UserSortingEnum.UserNameAsc:
                    allUser = allUser.OrderBy(q => q.UserName);
                    break;
                case UserSortingEnum.UserNameDesc:
                    allUser = allUser.OrderByDescending(q => q.UserName);
                    break;
                case UserSortingEnum.FullNameAsc:
                    allUser = allUser.OrderBy(q => q.FullName);
                    break;
                case UserSortingEnum.FullNameDesc:
                    allUser = allUser.OrderByDescending(q => q.FullName);
                    break;
                case UserSortingEnum.EmailAsc:
                    allUser = allUser.OrderBy(q => q.Email);
                    break;
                case UserSortingEnum.EmailDesc:
                    allUser = allUser.OrderByDescending(q => q.Email);
                    break;
                case UserSortingEnum.IsApprovedAsc:
                    allUser = allUser.OrderBy(q => q.IsApproved);
                    break;
                case UserSortingEnum.IsApprovedDesc:
                    allUser = allUser.OrderByDescending(q => q.IsApproved);
                    break;
                case UserSortingEnum.IsLockedOutAsc:
                    allUser = allUser.OrderBy(q => q.IsLockedOut);
                    break;
                case UserSortingEnum.IsLockedOutDesc:
                    allUser = allUser.OrderByDescending(q => q.IsLockedOut);
                    break;
                case UserSortingEnum.CreationDateAsc:
                    allUser = allUser.OrderBy(q => q.CreationDate);
                    break;
                case UserSortingEnum.CreationDateDesc:
                    allUser = allUser.OrderByDescending(q => q.CreationDate);
                    break;
            }
            return allUser.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            //if (string.IsNullOrWhiteSpace(name))
            //{
            //    return (from MembershipUser user in Membership.GetAllUsers(pageIndex, pageSize, out totalRecord) select ConvertTopCmsUser(user)).ToList();
            //}
            //return (from MembershipUser user in Membership.FindUsersByName("%" + name + "%", pageIndex, pageSize, out totalRecord) select ConvertTopCmsUser(user)).ToList();
        }

        public PCmsUser ConvertTopCmsUser(MembershipUser user)
        {
            var profile = AccountProfile.GetUserProfile(user.UserName);
            return new PCmsUser
                       {
                           Id = (Guid) user.ProviderUserKey,
                           UserName = user.UserName,
                           FullName = profile.FullName,
                           Address = profile.Address,
                           Email = user.Email,
                           ActivationType = profile.ActivationType,
                           IsApproved = user.IsApproved,
                           IsLockedOut = user.IsLockedOut,
                           PhoneNumber = profile.PhoneNumber,
                           TimeZoneId = profile.TimeZoneId,
                           Resale = profile.Resale,
                           Title = profile.Title,
                           BusinessDescription = profile.BusinessDescription,
                           BusinessName = profile.BusinessName,
                           TaxpaperId = profile.TaxpaperId,
                           CreationDate = user.CreationDate,
                           /*
                           AdditionalEmail = profile.AdditionalEmail,
                           ApplicantFirstName = profile.ApplicantFirstName,
                           ApplicantLastName = profile.ApplicantLastName,
                           BusinessType = profile.BusinessType,
                           CompanyAddress1 = profile.CompanyAddress1,
                           CompanyAddress2 = profile.CompanyAddress2,
                           CompanyCity = profile.CompanyCity,
                           CompanyName = profile.CompanyName,
                           CompanyType = profile.CompanyType,
                           SalesRepresentativeName = profile.SalesRepresentativeName,
                           ContactYou = profile.ContactYou,
                           FaxNumber = profile.FaxNumber,
                           ZipCode = profile.ZipCode,
                           FederalID = profile.FederalID,
                           DBA = profile.DBA,
                           PurcharseType = profile.PurcharseType,
                           StateProvince = profile.StateProvince
                            */
                       };
        }

        private string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}