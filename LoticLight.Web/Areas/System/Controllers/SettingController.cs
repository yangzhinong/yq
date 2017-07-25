using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web.Areas.System.Controllers
{
    public class SettingController : BaseController
    {
        // GET: System/Setting
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult GridData(Pagination pagination, string keyword)
        {
            var data = Business.Sms_SettingService.Instance.LoadPagerEntities(
                o => (o.Code.Contains(keyword) || o.Value.Contains(keyword) || string.IsNullOrEmpty(keyword)),
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

        #region 基础参数表单

        /// <summary>
        /// 基础参数表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Form(string keyValue)
        {
            ViewBag.Id = keyValue;
            return View();
        }

        /// <summary>
        /// 基础参数数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.Sms_SettingService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存基础参数信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult SaveForm(Model.Sms_Setting obj)
        {
            if (string.IsNullOrEmpty(obj.Id)) Business.Sms_SettingService.Instance.AddEntities(obj);
            else Business.Sms_SettingService.Instance.UpdateEntities(obj);
            return Success("保存成功");
        }
        /// <summary>
        /// 删除基础参数
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.Sms_SettingService.Instance.FindEntities(keyValue.Trim());
            Business.Sms_SettingService.Instance.DeleteEntities(obj);
            return Success("删除成功");
        }
        #endregion
    }
}