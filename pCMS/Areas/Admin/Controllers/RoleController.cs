using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using pCMS.Admin.Models;
using pCMS.Framework.Controllers;

namespace pCMS.Admin.Controllers
{
    public class RoleController : BaseAdminController
    {

        public ActionResult List()
        {
            var model = (from string role in Roles.GetAllRoles() select new RoleListModel() { RoleName = role }).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            var model = new RoleCreateOrUpdateModel();
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(RoleCreateOrUpdateModel model, bool continueEditing)
        {
            try
            {
                Roles.CreateRole(model.RoleName);
                SuccessNotification("Thêm mới nhóm '" + model.RoleName + "' thành công");
                return continueEditing ? RedirectToAction("Edit", new { id = model.RoleName }) : RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message, false);
            }
            return View(model);
        }
        public ActionResult Edit(string id)
        {
            if (!Roles.RoleExists(id)) return RedirectToAction("List");
            var model = new RoleCreateOrUpdateModel {RoleName = id, IsEdit = true};
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            try
            {
                Roles.RemoveUsersFromRole(Roles.GetUsersInRole(id), id);
                Roles.DeleteRole(id);
                SuccessNotification("Xóa nhóm '" + id + "' thành công");
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex.GetBaseException().Message);
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}
