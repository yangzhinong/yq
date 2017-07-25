using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSystem = global::System;

namespace LoticLight.Web.Areas.YQ.Controllers
{
    public class SBBTypeController : BaseController
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

            var data = Business.YqSBBTypeService.Instance.LoadPagerEntities(
                o => (o.Name.Contains(keyword) || string.IsNullOrEmpty(keyword)),
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
            return View();
        }

        [SimpleCheckPermission("admin")]
        public ActionResult SaveForm(Model.YqSBBType obj)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.Id = Guid.NewGuid().ToString();
                Business.YqSBBTypeService.Instance.AddEntities(obj);
            }
            else
            {
                Business.YqSBBTypeService.Instance.UpdateEntities(obj);
            }
            return Success("Save Data Success");
        }

        [NotCheckPermission]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.YqSBBTypeService.Instance.FindEntities(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [SimpleCheckPermission("admin")]
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.YqSBBTypeService.Instance.FindEntities(keyValue.Trim());
            if (obj != null)
            {
                Business.YqSBBTypeService.Instance.RemoveEntities(obj);
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
            return ExportExcel("select Name from YqSBBType order by Name", "SBB Type.xls", "SBB Type");
        }
    }
}