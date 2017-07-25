using LoticLight.Business;
using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSystem = global::System;

namespace LoticLight.Web.Areas.YQ.Controllers
{
    public class GSMDataDownController : BaseController
    {
        // GET: YQ/GSMDataDown
        public ActionResult Index()
        {
            return View();
        }

        [NotCheckPermission]
        public ActionResult download()
        {
            return ExportExcel("dbo.pDownloadGSMDataALL", "GSM FeedBack DownLoad.xls", "", GSystem.Data.CommandType.StoredProcedure,
                               new GSystem.Data.SqlClient.SqlParameter("@gsm", currentUseName));

        }
        [HttpPost]
        [SimpleCheckPermission("gsm")]
        [ValidateInput(false)]
        public ActionResult upLoad(HttpPostedFileBase file, string sheetName = "Sheet1")
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


            var lst = new List<Model.YqGSMFeedBack>();
            for (var iRow = 2; iRow <= rowcount; iRow++)  //从第2行开始
            {
                var sRow = iRow.ToString();
                try
                {
                    var oData = new YqGSMFeedBack();

                    oData.FeekBackTime = dNow;
                    oData.GSM = currentUseName;
                    oData.PN = sh.Cells["B" + sRow].StringValue;
                    oData.Location = sh.Cells["C" + sRow].StringValue;
                    oData.Feedback = sh.Cells["G" + sRow].StringValue;
                    oData.weekInt = weekInt;
                    if (string.IsNullOrWhiteSpace(oData.PN)) break;
                    if (!VaildData(oData))
                    {
                        return Error("第" + sRow + "数据有问题!请检查");
                    }

                    if (currentUseName != sh.Cells["A" + sRow].StringValue)
                    {
                        return Error("第" + sRow + "数据的GSM信息,不是当前用户!");
                    }

                    var oFind = lst.Where(x => x.PN == oData.PN && x.Location == oData.Location).FirstOrDefault();
                    if (oFind != null)
                    {
                        return Error("第" + sRow + "数据与前面的数据有重复!");
                    }

                    lst.Add(oData);

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

            var sqlDelOld = @"DELETE FROM dbo.YqGSMFeedBack 
                                 WHERE WeekInt= DATEDIFF(WEEK,0, GETDATE()) AND GSM=@gsm
                            ";
            SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(),
                                    GSystem.Data.CommandType.Text, 
                                    sqlDelOld, 
                                    new GSystem.Data.SqlClient.SqlParameter("@gsm", currentUseName));

            var i = Business.YqGSMFeedBackService.Instance.AddEntities(lst);

            return Success("我的回复上传成功!");
        }

        private bool VaildData(YqGSMFeedBack model)
        {
            if (string.IsNullOrWhiteSpace(model.PN) || (string.IsNullOrWhiteSpace(model.Location)) || string.IsNullOrWhiteSpace( model.Feedback))
            {
                return false;
            }
            else
            {
                return true;
            }
        }




    }
}