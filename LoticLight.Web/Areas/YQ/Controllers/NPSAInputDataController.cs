using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSystem = global::System;

namespace LoticLight.Web.Areas.YQ.Controllers
{
    public class NPSAInputDataController : BaseController
    {

        #region List

        [NotCheckPermission]
        public ActionResult Index()
        {
            var lstProjects = Business.YqProjectService.Instance.LoadEntities()
                            .Where(x=>x.NPSA == currentUseName)
                            .OrderBy(x => x.Name).ToList();

            ViewBag.lstProjects = lstProjects;
            return View();

        }
        /// <summary>
        /// 取得列表
        /// </summary>
        /// <returns></returns>
        [NotCheckPermission]
        public ActionResult GridData(Pagination pagination, string searchKey = "", string searchType="-1", string startTime="", string endTime="")
        {
            var npsa = LoticLight.Session.WebSession.CurrentAccount.User.UserName;
            var weekInt = Business.Basics.clsWeek.WeekInt();
            var oData = Business.YqViewNPSAInputDataService.Instance.LoadEntities(x=>x.WeekInt== weekInt);
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                switch (searchType)
                {
                    case "1":  //pn
                      oData=  oData.Where(x => x.PN.Contains(searchKey) || x.PNDesp.Contains(searchKey));
                        break;
                    case "2":
                        oData= oData.Where(x => x.Project.Contains(searchKey));
                        break;
                    case "3":
                        oData= oData.Where(x => x.Location.Contains(searchKey));
                        break;
                    case "4":
                        oData= oData.Where(x => x.GSM.Contains(searchKey));
                        break;
                    case "5":
                       oData=  oData.Where(x => x.Vendor.Contains(searchKey));
                        break;
                }
            }
            var d0 = Business.Basics.clsWeek.GetDateTimeFromString(startTime);
            var d1 = Business.Basics.clsWeek.GetDateTimeFromString(endTime);

            if (d0 == null)
            {
                d0 = DateTime.Now.Date.AddDays(-7);
            } 
            oData = oData.Where(x => x.API >= d0);
            if (d1 == null)
            {
                d1 = DateTime.Now.Date.AddMonths(3);
            }
            oData = oData.Where(x => x.API <= d1);

            return Json(JQGridHelper<dynamic>.GetPageData(oData, pagination)
                         , JsonRequestBehavior.AllowGet);
        }

        [NotCheckPermission]
        public ActionResult PnList(string keyword)
        {
            var lst = Business.YqPNService.Instance.LoadEntities()
                .Where(x=>(x.PN.Contains(keyword) || x.Desp.Contains(keyword)) && !string.IsNullOrWhiteSpace(keyword) )
                .Select(x => new
                {
                    x.PN,
                    x.Desp,
                    x.SBBType,
                    x.Vendor,
                    x.GSM,
                    FullName= x.PN+" : "+x.Desp
                }).OrderBy(x=>x.PN).ToList();

            return Json(new { value = lst }, JsonRequestBehavior.AllowGet);

            
        }

        #endregion

        #region Form
        [NotCheckPermission]
        public ActionResult Form(string keyValue = "")
        {
            ViewBag.Id = keyValue;
            var lstLocations = Business.YqLocationService.Instance.LoadEntities()
                              .OrderBy(x => x.Name)
                              .ToList();

            ViewBag.lstLocations = lstLocations;

            var lstProjects = Business.YqProjectService.Instance.LoadEntities()
                             .Where(x => x.NPSA == currentUseName)
                             .OrderBy(x => x.Name).ToList();

            ViewBag.lstProjects = lstProjects;


            return View();
        }

        [SimpleCheckPermission("npsa")]
        public ActionResult SaveForm(Model.YqNPSAInputData obj)
        {
            try
            {
                var weekInt = Business.Basics.clsWeek.WeekInt();
                var npsa = LoticLight.Session.WebSession.CurrentAccount.User.UserName;
                obj.InputDate = DateTime.Now;
                #region ValidData
                if (string.IsNullOrWhiteSpace(obj.PN) || !Business.Data.clsPN.ExistsPN(obj.PN))
                {
                    return Error("PN不正确 或不存在,请检查!");
                }
                if (obj.API == null )
                {
                    return Error("API不正确");
                }
                ValidINputByMode(obj, npsa, weekInt);
                #endregion

                if (string.IsNullOrEmpty(obj.Id))
                {

                    obj.Id = Guid.NewGuid().ToString();
                    obj.WeekInt = Business.Basics.clsWeek.WeekInt();
                    obj.NPSA = npsa;
                    Business.YqNPSAInputDataService.Instance.AddEntities(obj);
                }
                else
                {
                    if (weekInt != obj.WeekInt)
                    {
                        return Error("你只能修改本周录入数据");
                    }
                    if (obj.NPSA != npsa)
                    {
                        return Error("你只能修改自已的数据!");
                    }
                    Business.YqNPSAInputDataService.Instance.UpdateEntities(obj);
                }
                return Success("Save Data Success");
            } catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        /// <summary>
        /// 对每种方进行检查
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="npsa"></param>
        /// <param name="weekInt"></param>
        /// <param name="Mode"></param>
        private void ValidINputByMode(YqNPSAInputData obj,string npsa,int weekInt)
        {
            
            var oLastWeekdata = Business.YqNPSADataService.Instance.LoadEntities(
                                            x => x.API == obj.API && x.PN == x.PN &&
                                            x.Location == obj.Location &&
                                            x.WeekInt == (weekInt - 1) &&
                                            x.Project == obj.Project &&
                                            x.NPSA == npsa).FirstOrDefault();
            if (oLastWeekdata != null && obj.Mode=="ADD")
            {
                throw new Exception("上周已有该数据! 不能选用ADD模式!");
            }
            if (oLastWeekdata == null && obj.Mode == "CHANGE")
            {
                throw new Exception("上周没有该数据! 不能选用CHANGE模式!");
            }
            if (oLastWeekdata ==null && obj.Mode == "CANCEL")
            {
                throw new Exception("上周没有该数据! 不能选用CANCEL模式!");
            }
        }



        [NotCheckPermission]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = Business.YqNPSAInputDataService.Instance.FindEntities(keyValue);
            if (data != null)
            {
                var oPN = Business.YqPNService.Instance.LoadEntities(x => x.PN == data.PN).FirstOrDefault();
                if (oPN != null)
                {
                    data.PN = oPN.PN + " : " + oPN.Desp;
                }
            }
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Delete

        [SimpleCheckPermission("admin")]
        public ActionResult Delete(string keyValue)
        {
            var obj = Business.YqNPSAInputDataService.Instance.FindEntities(keyValue.Trim());
            if (obj != null)
            {
                Business.YqNPSAInputDataService.Instance.RemoveEntities(obj);
                return Success("Del Sucess");
            }
            else
            {
                return Error("Can't found the NPSA. PLease check it.");
            }

        }
        #endregion


        [NotCheckPermission]
        public ActionResult export()
        {
            var sql = @"SELECT [Mode],[PN],[SBBType],[PNDesp],[Project],[API],[Location],[Vendor],[WeekDesp],Demand,[GSM] 
FROM YqViewNPSAInputData 
WHERE NPSA=@npsa AND WeekInt= DATEDIFF(WEEK,0,GETDATE())
ORDER BY Mode,PN
";
            return ExportExcel(sql, "NPSA input data.xls", "", GSystem.Data.CommandType.Text,
                                 new GSystem.Data.SqlClient.SqlParameter("@npsa", currentUseName));

        }

        [NotCheckPermission]
        public ActionResult downCalResult()
        {
           
            return ExportExcel("dbo.pGetANPSAthisWeekDataResult", "本周录入计算结果.xls", "", GSystem.Data.CommandType.StoredProcedure,
                                 new GSystem.Data.SqlClient.SqlParameter("@npsa", currentUseName));

        }

        [SimpleCheckPermission("admin")]
        public ActionResult publishToGSM()
        {

            var i = Business.SqlHelper.ExecuteNonQuery(Business.SqlHelper.GetConnSting(), "dbo.pGenThisWeekNPSAData");

            if (i > 0)
            {
                return Success("发布成功!");
            }
            else
            {
                return Error("发布失败");
            }

         
        }


        [NotCheckPermission]
        public ActionResult DownloadToGSM()
        {
            //Business.SqlHelper.ExecuteNonQuery(Business.SqlHelper.GetConnSting(), "dbo.pDownloadGSMDataALL");
            //return ExportExcel("dbo.pGetANPSAthisWeekDataResult", "本周录入计算结果.xls", "", GSystem.Data.CommandType.StoredProcedure,
            //new GSystem.Data.SqlClient.SqlParameter("@npsa", currentUseName));
            return ExportExcel("dbo.pDownloadGSMDataALL", "Send To GMS.xls", "", GSystem.Data.CommandType.StoredProcedure,
                                 new GSystem.Data.SqlClient.SqlParameter("@gsm", "-1"));

        }

        [NotCheckPermission]
        public ActionResult WorkToGSM()
        {
            return View();
        }


        #region 某个Project替换

        [NotCheckPermission]
        public ActionResult ReplaceProject(HttpPostedFileBase file)
        {
            if (file == null) return JsonMsg.ErrMsg("你没有上传文件,无法操作!");

            var wb = new Aspose.Cells.Workbook(file.InputStream);
            var sh = wb.Worksheets[0];

            if (sh.Cells["A1"].StringValue != "PN")
            {
                return JsonMsg.ErrMsg("工作表格式不对!(A1单元格应为PN)");
            }
            var rowcount = sh.Cells.MaxRow + 1;
            var lstInExcel = new List<Model.YqNPSAData>();
            var sbError=new GSystem.Text.StringBuilder();
            var bError = false;
            var sProject = "";
            var dNow = DateTime.Now;
            var thisWeekInt = Business.Basics.clsWeek.WeekInt();
            #region 读取上传数据
            for (var iRow=2; iRow<=rowcount; iRow++)
            {
                var sRow = iRow.ToString();
                try
                {
                    var o = new YqNPSAData();
                    o.PN = sh.Cells["A" + sRow].StringValue;
                    o.Project = sh.Cells["C" + sRow].StringValue;
                    o.Location = sh.Cells["D" + sRow].StringValue;
                    o.API =DateTime.Parse( sh.Cells["E" + sRow].StringValue);
                    o.Demand = sh.Cells["F" + sRow].IntValue;
                    o.WeekInt = thisWeekInt;
                    o.NPSA = currentUseName;
                    if (string.IsNullOrWhiteSpace(o.PN)) break;
                    lstInExcel.Add(o);

                    if (iRow == 2)
                    {
                        sProject = o.Project;

                        if (Business.YqProjectService.Instance.LoadEntities(x=>x.Name==sProject).FirstOrDefault() == null)
                        {
                            throw new Exception("Project不存在,请检查!");
                        }
                    }
                    
                    if (Business.YqPnLocationService.Instance.LoadEntities(x=>x.PN == o.PN && x.Location== o.Location).FirstOrDefault()==null)
                    {
                        throw new Exception("该PN-Location关系不存在! 请检查!");
                    }

                    if (sProject != o.Project)
                    {
                        throw new Exception("同一张工作表只能含有一个Project的数据!");
                    }

                    if (Business.YqPNService.Instance.LoadEntities(x=>x.PN== o.PN).FirstOrDefault() == null)
                    {
                        throw new Exception("该PN不在基础资料中,请检查.");
                    }
                } catch (Exception ex)
                {
                    sbError.AppendLine("第" + sRow + "行:" + ex.Message);
                    bError = true;
                }
            }
            if (bError)
            {
                return JsonMsg.ErrMsg(  sbError.ToString());
            }

            #endregion

            if (lstInExcel.Count() < 1) return JsonMsg.ErrMsg("没有数据无法处理!");

            if (Business.YqProjectService.Instance.LoadEntities(x => x.NPSA == currentUseName && x.Name == sProject).FirstOrDefault() == null)
            {
                return JsonMsg.ErrMsg("不是你的Project不能操作!");
            }


            Business.SqlHelper.ExecuteNonQueryText("DELETE FROM	 dbo.YqNPSAData WHERE Project=@p AND WeekInt=@weekInt", 
                                                        new GSystem.Data.SqlClient.SqlParameter("@p", sProject),
                                                        new GSystem.Data.SqlClient.SqlParameter("@weekInt", thisWeekInt));



            Business.YqNPSADataService.Instance.AddEntities(lstInExcel);


            var iRet = Convert.ToInt32(Business.SqlHelper.ExecuteScalarCmd("dbo.pReplaceAProeject",
                                             new GSystem.Data.SqlClient.SqlParameter("@project", sProject)));

            if (iRet == 1)
            {
                return JsonMsg.OkMsg("替换Project成功");
            } else
            {
                return JsonMsg.OkMsg("替换Project失败!");
            }
            
        }


        [NotCheckPermission]
        public ActionResult DownLoadLastWeekForAProject(string project)
        {
               
            return ExportExcel("dbo.pGetLastWeekNPSAData", project + Business.Basics.clsWeek.LastWeekName() + ".xls", "", GSystem.Data.CommandType.StoredProcedure ,
                                new GSystem.Data.SqlClient.SqlParameter("@project", project));



        }


        [NotCheckPermission]
        public ActionResult DelAProject(string project)
        {
            if (string.IsNullOrWhiteSpace(project))
            {
                return JsonMsg.ErrMsg("没有选择Project,无法操作!");
            }

            if (Business.YqProjectService.Instance.LoadEntities(x=>x.NPSA== currentUseName && x.Name== project).FirstOrDefault() == null)
            {
                return JsonMsg.ErrMsg("不是你的Project不能操作!");
            }

            var iRet = Business.SqlHelper.ExecuteScalarCmd("dbo.pDelAProeject",
                   new GSystem.Data.SqlClient.SqlParameter("@project", project)
                );
            if (Convert.ToInt32(iRet) == 1)
            {
                return JsonMsg.OkMsg(string.Format("删除Project: {0} 成功!", project));
            } else
            {
                return JsonMsg.ErrMsg(string.Format("删除Project: {0} 失败!", project));
            }
        }
        #endregion


        #region 查询上周数据
        [NotCheckPermission]
        public ActionResult QueryLastWeekData(Pagination pagination, string pn = "", string api = "", string project = "", string location = "")
        {
            var npsa = LoticLight.Session.WebSession.CurrentAccount.User.UserName;
            var weekInt = Business.Basics.clsWeek.WeekInt();
            var oData = Business.YqViewNPSADataService.Instance.LoadEntities(x => x.WeekInt == weekInt-1 && x.NPSA== npsa);
            if (!string.IsNullOrWhiteSpace(pn)){
                oData = oData.Where(x => x.PN == pn);
            }
            if (!string.IsNullOrWhiteSpace(project))
            {
                oData = oData.Where(x => x.Project == project);
            }
            if (!string.IsNullOrWhiteSpace(location))
            {
                oData = oData.Where(x => x.Location == location);
            }

            DateTime? dAPI=null;
            try
            {
                dAPI = DateTime.Parse(api);
            }
            catch { }
            if (dAPI.HasValue)
            {
                oData = oData.Where(x => x.API == dAPI);
            }
            return Json(JQGridHelper<dynamic>.GetPageData(oData, pagination)
                         , JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region 领导数据
        [SimpleCheckPermission("admin,npsa")]
        public ActionResult DownLeadData()
        {
            return ExportExcel("dbo.pGenLeadData", "LeadData.xls", "", GSystem.Data.CommandType.StoredProcedure);

        }

        #endregion

    }
}