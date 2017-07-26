using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSystem = global::System;
using System.Data.SqlClient;
using LoticLight.Business;

namespace LoticLight.Web.Areas.YQ.Controllers
{
    public class PNController : BaseController
    {



        public ActionResult Index()
        {
            var lstGSM = Business.YqGSMService.Instance.LoadEntities()
                        .OrderBy(x => x.Name)
                        .ToList();
            ViewBag.lstGSM = lstGSM;
            return View();

        }
        /// <summary>
        /// 取得列表
        /// </summary>
        /// <returns></returns>
        [NotCheckPermission]
        public ActionResult GridData(Pagination pagination, string keyword = "")
        {
            var data = Business.YqViewPNService.Instance.LoadPagerEntities(
                o => (o.PN.Contains(keyword) || o.Desp.Contains(keyword) || string.IsNullOrEmpty(keyword)),
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
            var lstSBBType = Business.YqSBBTypeService.Instance.LoadEntities()
                                .OrderBy(x=>x.Name)
                                .ToList();
            ViewBag.Id = keyValue;
            ViewBag.lstSBBType = lstSBBType;
            var lstGSM = Business.YqGSMService.Instance.LoadEntities()
                         .OrderBy(x => x.Name)
                         .ToList();
            ViewBag.lstGSM = lstGSM;

            var lstVendor = Business.YqVendorService.Instance.LoadEntities()
                        .OrderBy(x => x.Name)
                        .ToList();
            ViewBag.lstVendor = lstVendor;

            

            var lstLocations = Business.YqLocationService.Instance.LoadEntities()
                              .OrderBy(x => x.Name)
                              .ToList();

            ViewBag.lstLocations = lstLocations;


            return View();
        }

        [SimpleCheckPermission("admin")]
        public ActionResult SaveForm(Model.YqPN obj,string Location, string GSM)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.Id = Guid.NewGuid().ToString();
                
                Business.YqPNService.Instance.AddEntities(obj);

                if (!string.IsNullOrWhiteSpace(Location))
                {
                    var aS = Location.Split(',');
                    foreach (var a in aS)
                    {
                        Business.YqPnLocationService.Instance.AddEntities(new YqPnLocation { PN = obj.PN, Location = a,GSM=GSM });
                    }
                }
            }
            else
            {
                var oldPn = Business.YqPNService.Instance.LoadEntitiesNoTracking(x=>x.Id==obj.Id).FirstOrDefault();
                Business.YqPNService.Instance.UpdateEntities(obj);
                var oldPnLocationList = Business.YqPnLocationService.Instance.LoadEntities(x => x.PN == oldPn.PN).ToList();
                Business.YqPnLocationService.Instance.RemoveEntities(oldPnLocationList);

                if (!string.IsNullOrWhiteSpace(Location))
                {
                    var aS = Location.Split(',');
                    foreach (var a in aS)
                    {
                        Business.YqPnLocationService.Instance.AddEntities(new YqPnLocation { PN = obj.PN, Location = a ,GSM=GSM});
                    }
                }

            }
            return Success("Save Data Success");
        }

        [NotCheckPermission]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.YqPNService.Instance.FindEntities(keyValue);

            var pnLocations = Business.YqPnLocationService.Instance.LoadEntities(x => x.PN == data.PN)
                              .Select(x => x.Location).ToArray();
            var pnGSMs = Business.YqPnLocationService.Instance.LoadEntities(x => x.PN == data.PN)
                             .Select(x => x.GSM).Distinct().ToString();
            var data2 = new
            {
                data.Id,
                data.PN,
                GSM = string.Join(",", pnGSMs),
                data.SBBType,
                data.Vendor,
                Location = string.Join(",", pnLocations),
                data.Desp
            };
            return Json(data2, JsonRequestBehavior.AllowGet);
        }
        [SimpleCheckPermission("admin")]
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.YqPNService.Instance.FindEntities(keyValue.Trim());
            if (obj != null)
            {
                Business.YqPNService.Instance.RemoveEntities(obj);
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
            var sql = @"SELECT a.PN, a.Desp,a.SBBType,c.Location, a.Vendor, a.GSM 
FROM dbo.YqPN a,  dbo.YqPnLocation c  
WHERE a.PN= c.PN ORDER BY a.PN";
            return ExportExcel(sql, "PN.xls", "");
        }

        [NotCheckPermission]
        public ActionResult UpLoad()
        {
            return View();
        }

        [HttpPost]
        [SimpleCheckPermission("admin")]
        [ValidateInput(false)]
        public ActionResult UpLoad(HttpPostedFileBase file,string sheetName="Sheet1")
        {
            if (file == null) return Error("你没有上传文件,无法操作!");
            var wb = new Aspose.Cells.Workbook(file.InputStream);
            var sh = wb.Worksheets[sheetName];
            if (sh == null)
            {
               return Error("指定的工作表不存于上传的文件中");
            }

            if (!(sh.Cells["A1"].StringValue == "L2 P/N"  || sh.Cells["A1"].StringValue=="PN"))
            {
                return Error("选定的工作表格式不对!");
            } 
            var rowcount = sh.Cells.MaxRow + 1;
            var dNow = DateTime.Now;
            var list = new List<ExcelInputModel>();
            var sbError = new GSystem.Text.StringBuilder();
            var bError = false;
            for (var iRow=2; iRow<= rowcount; iRow++)  //从第2行开始
            {
                try
                {
                    var oPn = new ExcelInputModel();

                    oPn.PN = sh.Cells[("A" + iRow.ToString())].StringValue;

                    
                    if (string.IsNullOrEmpty(oPn.PN) || string.IsNullOrWhiteSpace(oPn.PN))
                        break;

                    oPn.Desp = (sh.Cells[("B" + iRow.ToString())]).Value.ToString();
                    oPn.SBBType = (sh.Cells[("C" + iRow.ToString())]).Value.ToString();
                    oPn.Location = (sh.Cells[("D" + iRow.ToString())]).Value.ToString();
                    oPn.Vendor = (sh.Cells[("E" + iRow.ToString())]).Value.ToString();
                    oPn.GSM = (sh.Cells[("F" + iRow.ToString())]).Value.ToString();
                    if (!oPn.Valid())
                    {
                        bError = true;
                        sbError.AppendLine(string.Format("第{0}行,数据有误,请检查!", iRow + 1));
                    }
                    list.Add(oPn);
                }
                catch
                {
                    return Error("第" + iRow.ToString() + "数据有问题!请检查");
                }
            }
            if (bError)
            {
                return Error(sbError.ToString());
            }

            foreach(var p in list)
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.GetConnSting(),
                                GSystem.Data.CommandType.StoredProcedure,
                                "dbo.pImportPN",
                                new SqlParameter("@pn", p.PN),
                                new SqlParameter("@desp", p.Desp),
                                new SqlParameter("@sbbType", p.SBBType),
                                new SqlParameter("@gsm", p.GSM),
                                new SqlParameter("@location", p.Location),
                                new SqlParameter("@vendor", p.Vendor));
            }

            return Success("数据上传成功!");
        } 

        private class ExcelInputModel: Model.YqPN
        {
            public string Location { get; set; }
           

            public string GSM { get; set; }

            public bool Valid()
            {
                if (string.IsNullOrWhiteSpace(PN) || string.IsNullOrWhiteSpace(Desp)
                    || string.IsNullOrWhiteSpace(SBBType) || string.IsNullOrWhiteSpace(Location)
                    || string.IsNullOrWhiteSpace(Vendor) || string.IsNullOrWhiteSpace(GSM))
                    return false;
                else return true;
           
            }
        }
        [SimpleCheckPermission("admin")]
        public ActionResult ChangeGSM(string oldgsm, string newgsm)
        {
            if (oldgsm == newgsm)
            {
                return JsonMsg.ErrMsg("新旧GSM一样,无法进行替换操作");
            }
            if (string.IsNullOrEmpty(oldgsm) || string.IsNullOrEmpty(newgsm))
            {
                return JsonMsg.ErrMsg("请输入正确的GSM信息!");
            }
            try
            {
                var i = SqlHelper.ExecuteNonQueryText("UPDATE dbo.YqPN SET GSM=@newgsm WHERE GSM=@oldgsm",
                                                                 new SqlParameter("@oldgsm", oldgsm),
                                                                 new SqlParameter("@newgsm", newgsm));
                return JsonMsg.OkMsg("GSM替换成功!");
            }
            catch (Exception ex)
            {
                return JsonMsg.ErrMsg(ex.Message);
            }

        }
    }
}