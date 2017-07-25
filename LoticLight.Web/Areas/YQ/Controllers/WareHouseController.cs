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
    public class WareHouseController : BaseController
    {
        

        // GET: YQ/WareHouse
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [SimpleCheckPermission("admin")]
        [ValidateInput(false)]
        public ActionResult UpLoad(HttpPostedFileBase file, string sheetName = "Sheet1")
        {
            if (file == null) return Error("你没有上传文件,无法操作!");
            var wb = new Aspose.Cells.Workbook(file.InputStream);
            var sh = wb.Worksheets[sheetName];
            if (sh == null)
            {
                return Error("指定的工作表不存于上传的文件中");
            }

            if (sh.Cells["A1"].StringValue != "PN")
            {
                return Error("选定的工作表格式不对!");
            }
            var rowcount = sh.Cells.MaxRow + 1;
            var dNow = DateTime.Now;
            var sbError = new GSystem.Text.StringBuilder();
            var bError = false;
            var weekInt = Business.Basics.clsWeek.WeekInt();
           

            var lst = new List<Model.YqPnQty>();
            for (var iRow = 2; iRow <= rowcount; iRow++)  //从第2行开始
            {
                var sRow = iRow.ToString();
                try
                {
                    var oPNQty = new YqPnQty();
                    
                    oPNQty.ImportDate = dNow;
                    oPNQty.PN = sh.Cells["A" + sRow].StringValue;
                    oPNQty.Location = sh.Cells["B" + sRow].StringValue;
                    oPNQty.Qty = sh.Cells["C" + sRow].IntValue;
                    oPNQty.WeekInt = weekInt;
                    if (string.IsNullOrWhiteSpace(oPNQty.PN)) break;
                    if (!VaildData(oPNQty))
                    {
                        return Error("第" + sRow + "数据有问题!请检查");
                    }

                    var oFind= lst.Where(x => x.PN == oPNQty.PN && x.Location == oPNQty.Location).FirstOrDefault();
                    if (oFind != null)
                    {
                        return Error("第" + sRow + "数据与前面的数据有重复!");
                    }

                    lst.Add(oPNQty);
                    
                }
                catch
                {
                    return Error("第" + sRow + "数据有问题!请检查");
                }
            }

            if (bError)
            {
                return Error(sbError.ToString());
            }

            SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), GSystem.Data.CommandType.Text, "DELETE FROM dbo.YqPnQty WHERE	 WeekInt= DATEDIFF(WEEK,0, GETDATE())");

            var i=  Business.YqPnQtyService.Instance.AddEntities(lst);

            return Success("数据上传成功!");
        }

        private bool VaildData(YqPnQty model)
        {
            if (string.IsNullOrWhiteSpace(model.PN) || (string.IsNullOrWhiteSpace(model.Location)) || model.Qty < 0)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}