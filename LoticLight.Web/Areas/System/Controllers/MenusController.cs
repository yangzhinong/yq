using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web.Areas.System
{
    public class MenusController :BaseController
    {
        // GET: System/Menus




        #region 数据列表
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GridData(  string parentid,string keyword="")
        {

            var data = Business.Sys_MenuService.Instance
                .LoadEntities(o =>
                    (o.ParentId == parentid
                    || parentid=="-1")
                && (
                 o.MenuName.Contains(keyword)
                ||o.MenuCode.Contains(keyword)
                ||o.MenuHref.Contains(keyword)
                 )
                ).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
 
        }
       
        

        #endregion

        #region 表单

        public ActionResult Form(string keyValue)
        {
            ViewBag.Id = keyValue;
            return View();
        }
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.Sys_MenuService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Delete(string keyValue)
        {
            Business.Sys_MenuService.DeleteMenusAndChildren(keyValue);
            return Success("删除成功！");
        }
        public ActionResult SaveForm(string keyValue, Model.Sys_Menu Menu, string MenuActionsJson, string MenuViewsJson)
        {
            Business.Sys_MenuService.SaveMenus(keyValue, Menu, MenuActionsJson, MenuViewsJson);
            return Success("保存成功");
        }
      

        #region 功能操作


        /// <summary>
        /// 得到功能操作数据
        /// </summary>
        /// <param name="MenuId">菜单Id</param>
        /// <returns></returns>
        public ActionResult GetActionsData(string MenuId)
        {
            var data = Business.Sys_ActionService.Instance.LoadEntities(o => o.MenuId == MenuId.Trim()).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionForm()
        {
            return View();
        }

         #endregion

        #region 功能视图

        /// <summary>
        /// 得到功能视图数据
        /// </summary>
        /// <param name="MenuId">菜单Id</param>
        /// <returns></returns>
        public ActionResult GetViewsData(string MenuId)
        {
            var data = Business.Sys_GridViewService.Instance.LoadEntities(o => o.MenuId == MenuId.Trim()).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewForm()
        {
            return View();
        }


        
        #endregion

        #endregion
    
    
    }
}