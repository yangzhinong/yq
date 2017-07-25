using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSystem = global::System;

namespace LoticLight.Web.Areas.YQ.Controllers
{
    public class WeekController : BaseController
    {



        public ActionResult Index()
        {
            return View();

        }
        /// <summary>
        /// 取得列表
        /// </summary>
        /// <returns></returns>
        [NotCheckPermission]
        public ActionResult GridData(Pagination pagination, string keyword = "")
        {

            var data = Business.YqWeekService.Instance.LoadPagerEntities(
                o => (o.Desp.Contains(keyword) || string.IsNullOrEmpty(keyword)),
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



        [NotCheckPermission]
        public ActionResult Form(string keyValue = "")
        {
            ViewBag.Id = keyValue;
            ViewBag.monday = Business.Basics.clsWeek.GetMonday().ToString("yyyy-MM-dd");
            ViewBag.friday = Business.Basics.clsWeek.GetFirday().ToString("yyyy-MM-dd");
            return View();
        }

        [SimpleCheckPermission("admin")]
        public ActionResult SaveForm(Model.YqWeek obj)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                var WeekInt = Business.Basics.clsWeek.WeekInt();

                var oWeek = Business.YqWeekService.Instance.LoadEntities(x => x.WeekInt == WeekInt).FirstOrDefault();
                if (oWeek != null) 
                    return Error("已经有该周的信息!");

                obj.WeekInt = WeekInt;
                obj.Id = Guid.NewGuid().ToString();
                Business.YqWeekService.Instance.AddEntities(obj);
            }
            else
            {
                Business.YqWeekService.Instance.UpdateEntities(obj);
            }
            return Success("Save Data Success");
        }

        [NotCheckPermission]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.YqWeekService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [SimpleCheckPermission("admin")]
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.YqWeekService.Instance.FindEntities(keyValue.Trim());
            if (obj != null)
            {
                Business.YqWeekService.Instance.RemoveEntities(obj);
                return Success("Del Sucess");
            }
            else
            {
                return Error("Can't found the NPSA. PLease check it.");
            }

        }

        [NotCheckPermission]
        public ActionResult export()
        {
            return ExportExcel("SELECT Desp, CONVERT(NVARCHAR(10),Monday,120) AS Monday, CONVERT(NVARCHAR(10), Friday,120) AS Friday FROM dbo.YqWeek ORDER BY WeekInt DESC", "Week.xls", "Week");
        }
    }
}