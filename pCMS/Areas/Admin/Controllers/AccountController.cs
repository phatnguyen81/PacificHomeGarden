using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.Web.Mvc;
using pCMS.Admin.Models;
using pCMS.Core;
using pCMS.Core.Domain;
using pCMS.Core.Utils;
using pCMS.Framework.Controllers;
using pCMS.Framework.Helpers;
using pCMS.Services;

namespace pCMS.Admin.Controllers
{
    public class AccountController : BaseAdminController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IUserService _userService;
        private readonly IExportManager _exportManager;

        public AccountController(ILocalizationService localizationService, IUserService userService, IExportManager exportManager)
        {
            _localizationService = localizationService;
            _userService = userService;
            _exportManager = exportManager;
        }

        #region Ajax

        public JsonResult Unlock(string id)
        {
            try
            {
                var user = Membership.GetUser(id);
                if (user == null)
                    throw new Exception(
                        string.Format(_localizationService.GetResource("Admin.Accounts.Messages.Existed"), id));
                user.UnlockUser();
                Membership.UpdateUser(user);
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message });
            }
        }
        public UserSortingEnum GetUserSorting(SortDescriptor sortDescriptor)
        {
            if (sortDescriptor == null) return UserSortingEnum.CreationDateDesc;
            if(sortDescriptor.Member.Equals("UserName",StringComparison.CurrentCultureIgnoreCase))
            {
                return sortDescriptor.SortDirection == ListSortDirection.Ascending ? UserSortingEnum.UserNameAsc : UserSortingEnum.UserNameDesc;
            }
            if (sortDescriptor.Member.Equals("FullName", StringComparison.CurrentCultureIgnoreCase))
            {
                return sortDescriptor.SortDirection == ListSortDirection.Ascending ? UserSortingEnum.FullNameAsc : UserSortingEnum.FullNameDesc;
            }
            if (sortDescriptor.Member.Equals("Email", StringComparison.CurrentCultureIgnoreCase))
            {
                return sortDescriptor.SortDirection == ListSortDirection.Ascending ? UserSortingEnum.EmailAsc : UserSortingEnum.EmailDesc;
            }
            if (sortDescriptor.Member.Equals("IsApproved", StringComparison.CurrentCultureIgnoreCase))
            {
                return sortDescriptor.SortDirection == ListSortDirection.Ascending ? UserSortingEnum.IsApprovedAsc : UserSortingEnum.IsApprovedDesc;
            }
            if (sortDescriptor.Member.Equals("IsLockedOut", StringComparison.CurrentCultureIgnoreCase))
            {
                return sortDescriptor.SortDirection == ListSortDirection.Ascending ? UserSortingEnum.IsLockedOutAsc : UserSortingEnum.IsLockedOutDesc;
            }
            if (sortDescriptor.Member.Equals("CreationDate", StringComparison.CurrentCultureIgnoreCase))
            {
                return sortDescriptor.SortDirection == ListSortDirection.Ascending ? UserSortingEnum.CreationDateAsc : UserSortingEnum.CreationDateDesc;
            }
            return UserSortingEnum.CreationDateDesc;
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Accounts(GridCommand command, string SearchType, string Keyword)
        {
            
            List<PCmsUser> users;
            var totalRecord = 0;
            if (SearchType == "NAME")
            {
                users = _userService.FindUserByName(Keyword,GetUserSorting(command.SortDescriptors.FirstOrDefault()),  command.Page - 1, command.PageSize, out totalRecord);
            }
            else
            {
                users = _userService.FindUserByEmail(Keyword, GetUserSorting(command.SortDescriptors.FirstOrDefault()), command.Page - 1, command.PageSize, out totalRecord);
            }
            var model = new GridModel<AccountModel>
            {
                Data = users.Select(
                    q => new AccountModel()
                    {
                        UserName = q.UserName,
                        FullName = q.FullName,
                        Email = q.Email,
                        IsApproved = q.IsApproved,
                        IsLockedOut = q.IsLockedOut,
                        CreationDate = q.CreationDate
                    }).ToList(),
                Total = totalRecord
            };
          
            return new JsonResult
            {
                Data = model
            };
        }
        #endregion

        #region Account
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(AccountListModel model)
        {
            var users = new List<PCmsUser>();
            var totalRecord = 0;
            if(model.SearchType == "NAME")
            {
                users = _userService.FindUserByName(model.Keyword,GetUserSorting(null),  0, 20, out totalRecord);
            }
            else
            {
                users = _userService.FindUserByEmail(model.Keyword, GetUserSorting(null), 0, 20, out totalRecord);
            }
            model.Accounts = new GridModel<AccountModel>
                                 {
                                     Data = users.Select(
                                         q => new AccountModel()
                                                  {
                                                      UserName = q.UserName,
                                                      FullName = q.FullName,
                                                      Email = q.Email,
                                                      IsApproved = q.IsApproved,
                                                      IsLockedOut = q.IsLockedOut,
                                                      CreationDate = q.CreationDate
                                                  }).ToList(),
                                     Total = totalRecord

                                 };
            return View(model);

            // danh sách user
            /*
            var model = (from MembershipUser user in Membership.GetAllUsers()
                         select new AccountListModel
                                    {
                                        UserName = user.UserName,
                                        FullName = AccountProfile.GetUserProfile(user.UserName).FullName,
                                        Email = user.Email,
                                        IsApproved = user.IsApproved,
                                        IsLockedOut = user.IsLockedOut,
                                        
                                    }).ToList();
            return View(model);
             * */
        }
        
        [HttpPost]
        public ActionResult List(string[] rowitem)
        {
            if(rowitem == null || !rowitem.Any())
            {
                SuccessNotification(_localizationService.GetResource("Admin.Accounts.Messages.NoRowSelected"));
            }
            else
            {
                try
                {
                    foreach (var username in rowitem)
                    {
                        Membership.DeleteUser(username);
                    }
                    TempData["SuccessMessage"] = _localizationService.GetResource("Admin.Accounts.Messages.Deleted");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error : " + ex.GetBaseException().Message;
                }
            }
            
           
            return RedirectToAction("List");
        }

        public ActionResult Create()
        {
            var model = new AccountCreateOrUpdateModel();
            return View(model);
        }


        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(AccountCreateOrUpdateModel model,string[] rowitem, bool continueEditing)
        {
            try
            {
                //var user = Membership.CreateUser(model.UserName, model.Password);
                //user.IsApproved = model.IsApproved;
                //user.Email = model.Email;
                //Membership.UpdateUser(user);
                //var profile = AccountProfile.GetUserProfile(model.UserName);
                //profile.FullName = model.FullName;
                //profile.TimeZoneId = model.TimeZoneId;
                //profile.Address = model.Address;
                //profile.ActivationType = model.ActivationType;
                //profile.PhoneNumber = model.PhoneNumber;
                //profile.Save();

                var newUser = new PCmsUser
                {
                    UserName = model.UserName,
                    ActivationType = model.ActivationType,
                    //Address = model.Address,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    Password = model.Password,
                    Title = model.Title,
                    BusinessName = model.BusinessName,
                    BusinessDescription = model.BusinessDescription,
                    Resale = model.Resale,
                    TaxpaperId = model.TaxpaperId,
                    Address = model.Address

                    /*
                    AdditionalEmail = model.AdditionalEmail,
                    ApplicantFirstName = model.ApplicantFirstName,
                    ApplicantLastName = model.ApplicantLastName,
                    BusinessType = model.BusinessType.ToList(),
                    
                    CompanyAddress1 = model.CompanyAddress1,
                    CompanyAddress2 = model.CompanyAddress2,
                    CompanyCity = model.CompanyCity,
                    CompanyName = model.CompanyName,
                    CompanyType = model.CompanyType,
                    SalesRepresentativeName = model.SalesRepresentativeName,
                    ContactYou = model.ContactYou,
                    FaxNumber = model.FaxNumber,
                    ZipCode = model.ZipCode,
                    
                    FederalID = model.FederalID,
                    DBA = model.DBA,
                    
                    PurcharseType = model.PurcharseType,
                    StateProvince = model.StateProvince
                     */
                };
                _userService.Add(newUser);

                if(rowitem != null && rowitem.Length > 0)
                {
                    Roles.AddUserToRoles(newUser.UserName, rowitem);
                }

                SuccessNotification(string.Format(_localizationService.GetResource("Admin.Accounts.Messages.Added"), model.UserName));
                return continueEditing ? RedirectToAction("Edit", new { id = newUser.UserName }) : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            
            return View(model);
        }
        public ActionResult Edit(string id)
        {
            var user = _userService.GetUser(id);
            //var user = Membership.GetUser(id);
            if (user == null) return RedirectToAction("List");

            //var profile = AccountProfile.GetUserProfile(user.UserName);
            var model = new AccountCreateOrUpdateModel
                            {
                                ProviderUserKey = user.Id,
                                UserName = user.UserName,
                                FullName = user.FullName,
                                Email = user.Email,
                                IsApproved = user.IsApproved,
                                SelectedRoles = Roles.GetRolesForUser(user.UserName),
                                TimeZoneId = user.TimeZoneId,
                                PhoneNumber = user.PhoneNumber,
                                CreationDate = DateTimeHelpers.ConvertLocalToUserTimeZone(user.CreationDate),
                                //Address = profile.Address,
                                ActivationType = user.ActivationType,
                                IsLockedOut = user.IsLockedOut,
                                Title = user.Title,
                                BusinessName = user.BusinessName,
                                BusinessDescription = user.BusinessDescription,
                                Resale = user.Resale,
                                TaxpaperId = user.TaxpaperId,
                                Address = user.Address
                                /*
                                AdditionalEmail = profile.AdditionalEmail,
                                ApplicantFirstName = profile.ApplicantFirstName,
                                ApplicantLastName = profile.ApplicantLastName,
                                BusinessType = profile.BusinessType.ToArray(),
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
            return View(model);
        }

        [HttpPost]
        public ActionResult Approve(string id)
        {
            var user = Membership.GetUser(id);
            if (user == null) return RedirectToAction("List");
            try
            {
                user.IsApproved = true;
                Membership.UpdateUser(user);
                

                try
                {
                    EmailHelper.SendMailWithSignature(user.Email, "Approved", "RegisterSuccessEmail.htm", user.UserName);
                    SuccessNotification("Approve '" + user.UserName + "' successful!!!");
                }
                catch (Exception ex)
                {
                    ErrorNotification("Approve '" + user.UserName + "' successful but cannot send message ( Error : " +
                                      ex.GetBaseException().Message + ")");
                }

                return RedirectToAction("Edit", new {id = user.UserName});
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("List");
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(string id, AccountCreateOrUpdateModel model, string[] rowitem, bool continueEditing)
        {
            var user = _userService.GetUser(id);
            if (user == null) return RedirectToAction("List");
            try
            {
                user.UserName = model.UserName;
                user.ActivationType = model.ActivationType;
                //user.Address = model.Address;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.FullName = model.FullName;
                user.Password = model.Password;
                user.TaxpaperId = model.TaxpaperId;
                user.BusinessName = model.BusinessName;
                user.BusinessDescription = model.BusinessDescription;
                user.Title = model.Title;
                user.Resale = model.Resale;
                /*
                user.AdditionalEmail = model.AdditionalEmail;
                user.ApplicantFirstName = model.ApplicantFirstName;
                user.ApplicantLastName = model.ApplicantLastName;
                user.BusinessType = model.BusinessType.ToList();
                
                user.CompanyAddress1 = model.CompanyAddress1;
                user.CompanyAddress2 = model.CompanyAddress2;
                user.CompanyCity = model.CompanyCity;
                user.CompanyName = model.CompanyName;
                user.CompanyType = model.CompanyType;
                user.SalesRepresentativeName = model.SalesRepresentativeName;
                user.ContactYou = model.ContactYou;
                user.FaxNumber = model.FaxNumber;
                user.ZipCode = model.ZipCode;
                
                user.FederalID = model.FederalID;
                user.DBA = model.DBA;
                
                user.PurcharseType = model.PurcharseType;
                user.StateProvince = model.StateProvince;
                 */
                _userService.Update(user);

                if(!string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    _userService.ChangePassword(user.UserName, model.NewPassword);
                }


                // xóa danh sách roles
                if (Roles.GetRolesForUser(user.UserName).Length > 0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                }

                // thêm danh sách quyền
                if (rowitem != null && rowitem.Length > 0)
                {
                    Roles.AddUserToRoles(user.UserName, rowitem);
                }

                SuccessNotification(string.Format(_localizationService.GetResource("Admin.Accounts.Messages.Updated"), model.UserName));
                return continueEditing ? RedirectToAction("Edit", new { id = user.UserName }) : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                Membership.DeleteUser(id);
                SuccessNotification(string.Format(_localizationService.GetResource("Admin.Accounts.Messages.Deleted"), id));
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return RedirectToAction("Edit", new { id });
        }
        #endregion

        public void ExportExcelAll(string order)
        {

            try
            {
                UserSortingEnum orderEnum;
                if(!Enum.TryParse(order,true,out orderEnum))
                {
                    orderEnum = UserSortingEnum.CreationDateDesc;
                }
                var total = 0;
                var users = _userService.FindUserByEmail(null, orderEnum, 0, int.MaxValue, out total);

                var fileName = string.Format("customers_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var bytearray = _exportManager.ExportCustomersToXlsx(users);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                Response.BinaryWrite(bytearray);
                Response.Flush();
                Response.Close();
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
               // return RedirectToAction("List");
            }
        }
    }
}
