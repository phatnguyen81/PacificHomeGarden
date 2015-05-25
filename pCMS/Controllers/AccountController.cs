using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using pCMS.Core;
using pCMS.Core.Domain;
using pCMS.Framework;
using pCMS.Framework.Helpers;
using pCMS.Models;
using pCMS.Services;
using pCMS.Utils;
namespace pCMS.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWebHelper _webHelper;

        public AccountController(IUserService userService, IWebHelper webHelper)
        {
            _userService = userService;
            _webHelper = webHelper;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            //var user = Membership.GetUser("admin");
            //user.UnlockUser();
            //user.ChangePassword(user.ResetPassword(), "123456");
            //Membership.UpdateUser(user);
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            //SessionManager.CurrentShoppingCard.ClearAll();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
                                          Resale = model.Resale,
                                          BusinessName = model.BusinessName,
                                          BusinessDescription = model.BusinessDescription,
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
                    //AddNotification(NotifyType.Success, "Register successfully! Please contact with administrator to approve this account.", true);
                    AddNotification(NotifyType.Success, "Thank you for registering with Pacific Home and Garden! Please contact customer service at 1-866-439-1583 if you choose to activate by phone or send us an e-mail at info@pacifichomeandgarden.com if you choose to activate by email.", true);
                    try
                    {
                        EmailHelper.SendMail(AppSettings.EmailReceive, "New user register", string.Format("User '{0}' have just registered, please approve it !!!", model.UserName));
                    }
                    catch
                    {

                    }
                    return RedirectToAction("LogOn", "Account");
                }
                catch (Exception ex)
                {
                    AddNotification(NotifyType.Error, ex.GetBaseException().Message,true);
                }

                //ModelState.AddModelError("", ErrorCodeToString(createStatus));
                //// Attempt to register the user
                //MembershipCreateStatus createStatus;
                //Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);
                //var profile = AccountProfile.GetUserProfile(model.UserName);
                //profile.PhoneNumber = model.PhoneNumber;
                //profile.Address = model.Address;
                //profile.FullName = model.FullName;
                //profile.ActivationType = model.ActivationType;
                //profile.Save();
                //if (createStatus == MembershipCreateStatus.Success)
                //{
                //    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                //    AddNotification(NotifyType.Success, "Register successfully! Please contact with administrator to approve this account.", true);
                //    try
                //    {
                //        EmailHelper.SendMail(AppSettings.EmailReceive, "New user register", string.Format("User '{0}' have just registered, please approve it !!!", model.UserName));
                //    }
                //    catch 
                //    {
                        
                //    }
                    
                //    return RedirectToAction("LogOn", "Account");
                //}
                //ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
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

        public ActionResult ForgotPassword()
        {
            var model = new ForgotPasswordModel();
            return View(model);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            var users = Membership.FindUsersByEmail(model.Email);
            if(users.Count == 0 )
            {
                ErrorNotification("Cannot find user by this email");
            }
            else
            {
                try
                {
                    foreach (MembershipUser user in users)
                    {
                        var token = _userService.CreateTokenResetPassword(user.UserName);
                        var linkconfrim = _webHelper.GetRootUrl() + "/Account/ConfirmResetPassword/" + user.UserName + "?token=" + token;
                        EmailHelper.SendMailWithSignature(user.Email, "Confirm reset password", "ConfirmResetPassword.htm", user.UserName, linkconfrim);
                    }
                    SuccessNotification("Please check your email to confirm reset password !!!");
                    return RedirectToAction("ForgotPassword");
                }
                catch (Exception ex)
                {
                    ErrorNotification("Cannot reset password, please contact administrator !!!");
                }
            }
            return View(model);
        }

        public ActionResult ConfirmResetPassword(string id, Guid token)
        {
            var storetoken = _userService.GetTokenResetPassword(id);
            var user = _userService.GetUser(id);
            if (storetoken == Guid.Empty || storetoken != token)
            {
                return View(new ConfirmResetPasswordModel { Message = "Cannot reset password !!!" });
            }
            var newpassword = System.IO.Path.GetRandomFileName();
            newpassword = System.IO.Path.GetFileNameWithoutExtension(newpassword);
            _userService.ChangePassword(id, newpassword);
            EmailHelper.SendMailWithSignature(user.Email, "Reset password", "ResetPassword.htm", newpassword);

            return View(new ConfirmResetPasswordModel { Message = "Reset password successful, please check your email to get new pasword !!!" });
        }
        #endregion

        
    }
}
