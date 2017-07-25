using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoticLight.Business;
using LoticLight.Utility;

namespace LoticLight.Web.Areas.System.Controllers
{
    public class DepartmentController : BaseController
    {
        // GET: System/Department
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 取得信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GridData(string keyword = "")
        {
            var treeList = Business.Sys_DepartmentService.GetTreeList(keyword);
            return Content(treeList.TreeJson());
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.Sys_DepartmentService.Instance.FindEntities(keyValue.Trim());
            obj.IsDelete = true;
            Business.Sys_DepartmentService.Instance.UpdateEntities(obj);             
            return Success("删除成功");

        }
        /// <summary>
        /// 部门信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.Sys_DepartmentService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取所有上级产品类型
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult TreeData(string keyValue = "")
        {
            var data = Business.Sys_DepartmentService.GetTree("0", Business.Sys_DepartmentService.Instance.LoadEntities(o => o.Id != keyValue).ToList());
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Form(string keyValue="")
        {
            String flag="DP";
            
            int i=0;
             List<Sms_Setting> obj = Business.Sms_SettingService.Instance.LoadEntities(o => o.Code == "DepartmentNO").ToList();
            
             if (obj.Count >= 1)
                {
                    flag= obj[0].Value ;
                }
             if (flag.Length > 2)
             {
                 return Error("请设置部门编码前缀为2位字符！");
             }
            if (keyValue == "")
            {
                try
                {
                    if (Business.Sys_DepartmentService.Instance.LoadEntities().ToList().Count > 0)
                        i = Business.Sys_DepartmentService.Instance.LoadEntities().Max(o => Convert.ToInt32(o.code.Substring(2, o.code.Length - 2))) + 1;
                    else
                        i = 1;
                }
                catch
                {
                    flag = "系统编码错误！";
                    return Error(flag);
                    
                }
            }             
            ViewBag.code = flag + i.ToString();
            ViewBag.Id = keyValue;
            return View();
        }
        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult SaveForm(Model.Sys_Department obj)
        {
            string nowid = LoticLight.Session.WebSession.CurrentAccount.User.UserName;
            obj.updator = nowid;
            obj.updatetime = DateTime.Now;
            if (obj.ParentId == null)
                obj.ParentId = "0";
            if (string.IsNullOrEmpty(obj.Id))
            {
                //当前登录人ID
               
                //获取操作人IP地址
               
                if (obj.ParentId == null)
                    obj.ParentId = "0";
                obj.createtime = DateTime.Now;
                obj.creator = nowid;
            
              
                Business.Sys_DepartmentService.Instance.AddEntities(obj);
            }
            else

                Business.Sys_DepartmentService.Instance.UpdateEntities(obj);
            return Success("保存成功");
        }

    }
}