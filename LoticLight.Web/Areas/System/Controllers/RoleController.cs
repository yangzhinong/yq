using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web.Areas.System.Controllers
{
    public class RoleController : BaseController
    {
        // GET: System/Role
        #region 数据列表

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 角色列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult GridData(Pagination pagination, string keyword)
        {
            var data = Business.Sys_RoleService.Instance.LoadPagerEntities(
                o => (o.RoleName.Contains(keyword) || string.IsNullOrEmpty(keyword)),
                pagination.page, pagination.rows, out pagination.records,
                new OrderModelField() { propertyName = pagination.sidx, IsDESC = pagination.sord == "desc" ? true : false }
                );
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,

            };
            return Json(JsonData, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 角色列表数据
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public ActionResult GetRoleListJson(string userId)
        {
            var data = Business.Sys_RoleService.GetRoleListByUserId(userId);
            
            return Json(data,JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveUserRole(string userId, string roleIds)
        {
            Business.Sys_RoleService.SaveUserRole(userId, roleIds);
            return Success("保存成功");
        }

        #endregion
        #region 角色表单

        /// <summary>
        /// 角色信息表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Form(string keyValue)
        {
            ViewBag.Id = keyValue;
            return View();
        }

        /// <summary>
        /// 角色表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.Sys_RoleService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult SaveForm(Model.Sys_Role obj)
        {
            if (string.IsNullOrEmpty(obj.Id)) Business.Sys_RoleService.Instance.AddEntities(obj);
            else Business.Sys_RoleService.Instance.UpdateEntities(obj);
            return Success("保存成功");
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.Sys_RoleService.Instance.FindEntities(keyValue.Trim());
            Business.Sys_RoleService.Instance.DeleteEntities(obj);
            return Success("删除成功");

        }
 #endregion
       
        #region 权限分配

        /// <summary>
        /// 角色权限分配
        /// </summary>
        /// <returns></returns>
        public ActionResult authorizeForm()
        {
            return View();
        }
        /// <summary>
        /// 角色菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleMenus(string roleId)
        {
            var data = Business.Sys_MenuService.GetRoleMenusTree(roleId);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 角色操作
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult GetRoleActions(string roleId)
        {
            var data = Business.Sys_MenuService.GetRoleMenusActionsTree(roleId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 角色视图
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult GetRoleViews(string roleId)
        {
            var data = Business.Sys_MenuService.GetRoleMenusViewsTree(roleId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="moduleIds">菜单权限</param>
        /// <param name="moduleButtonIds">操作权限</param>
        /// <param name="moduleColumnIds">视图权限</param>
        /// <returns></returns>
        public ActionResult SaveAuthorize(string roleId, string moduleIds, string moduleButtonIds, string moduleColumnIds)
        {
            Business.Sys_MenuService.SaveAuthorize(roleId, moduleIds, moduleButtonIds, moduleColumnIds);
            return Success("保存成功！");
        }

        #endregion
        #region 用户分配
        //todo:用户分配未实现
        #endregion

       
    }
}