using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web.Areas.System.Controllers
{
    public class UserController : BaseController
    {
        // GET: System/Role
        #region 数据列表

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 用户列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult GridData(Pagination pagination, string keyword)
        {
            var data = Business.Sys_UserService.Instance.LoadPagerEntities(
                o => (o.UserName.Contains(keyword) || string.IsNullOrEmpty(keyword)),
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

        #endregion
        #region 用户表单

        /// <summary>
        /// 用户信息表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Form(string keyValue)
        {
            ViewBag.Id = keyValue;
            return View();
        }

        /// <summary>
        /// 用户表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.Sys_UserService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult SaveForm(Model.Sys_User obj)
        {
            bool b = Business.Sys_UserService.SaveUser(obj);
            if (b)
                return Success("保存成功");
            else
                return Error("登陆名重复");

        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.Sys_UserService.Instance.FindEntities(keyValue.Trim());
            Business.Sys_UserService.Instance.DeleteEntities(obj);
            return Success("删除成功");

        }
        #endregion

        /// <summary>
        /// 指派角色
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignRoleForm()
        {
            return View();
        }

        /// <summary>
        /// 用户角色数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignRoleData()
        {
            return View();
        }
        public ActionResult UserName()
        {
            var data = Business.Sys_UserService.Instance.LoadEntities();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}