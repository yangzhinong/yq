using LoticLight.Log;
using LoticLight.Model;
using LoticLight.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LoticLight.Business;

namespace LoticLight.Web
{
  public  class BaseController : Controller
    {
        protected string currentUseName = "";
        protected string hostUrl = "";

        public BaseController()
        {
            try
            {
                currentUseName= LoticLight.Session.WebSession.CurrentAccount.User.UserName;
            }
            catch { }

            if (string.IsNullOrWhiteSpace(currentUseName))
            {
                Redirect("~/Login/Login");
            }
        }

        #region 权限控制
         //<summary>
         //Action执行前
         //</summary>
         //<param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = LoticLight.Session.WebSession.CurrentUser;
            if (user == null)
            {
                var usernew = Business.Sys_UserService.Instance.LoadEntitiesNoTracking(o => o.Id == "8d71b215-15ed-44c2-bbf9-5241d65e41a5").FirstOrDefault();
                Model.AccountInfo account = new Model.AccountInfo();
                account.User = usernew;
                LoticLight.Session.WebSession.SetCurrentAccount(account);
                return;
            }
            base.OnActionExecuting(filterContext);

            string path = filterContext.HttpContext.Request.Path;
            if (!checkLogin() && 1 == 1)
            {
                filterContext.HttpContext.Response.Write(
                 " <script type='text/javascript'> alert('登录已过期，请重新登录'); window.top.location='/Login/Login';</script>");
                filterContext.RequestContext.HttpContext.Response.End();
                filterContext.Result = new EmptyResult();
                return;

            }

            if (gzNotCheck(filterContext))
            {
                WriteLog(path, filterContext);
                return;
            }

            if (IsSimpleCheck(filterContext))
            {
                WriteLog(path, filterContext);
                return;
            }

            if (!checkPermissions(path) && 1 == 1)
            {
                //filterContext.HttpContext.Response.Write(
                //  " <script type='text/javascript'> alert('您没有权限');</script>");

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var res = new JsonResult();
                    res.Data = new { code = 0, msg = "没有权限执行此功能!" };
                    filterContext.Result = res;
                } else
                {
                    filterContext.HttpContext.Response.Write(
                    "您没有权限");
                    filterContext.RequestContext.HttpContext.Response.End();
                    filterContext.Result = new EmptyResult();
                    return;
                }
                
            }
            
            
            WriteLog(path, filterContext);
        }

        private bool gzNotCheck(ActionExecutingContext filterContext)
        {
            var x = filterContext.ActionDescriptor.GetCustomAttributes(typeof (NotCheckPermissionAttribute), false).Cast<NotCheckPermissionAttribute>();

            return (x != null && x.Count() > 0);

        }

        private bool IsSimpleCheck(ActionExecutingContext filterContext)
        {
            var permissionAttributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(SimpleCheckPermissionAttribute), false).Cast<SimpleCheckPermissionAttribute>();
            permissionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(SimpleCheckPermissionAttribute), false).Cast<SimpleCheckPermissionAttribute>().Union(permissionAttributes);
            var attributes = permissionAttributes as IList<SimpleCheckPermissionAttribute> ?? permissionAttributes.ToList();
            if (!(permissionAttributes != null && attributes.Count() > 0)) return false;

            var u = LoticLight.Session.WebSession.CurrentUser;

            var myRoles=  Business.Sys_RoleService.GetRoleListByUserId2(u.Id);

            var permis0 = permissionAttributes.FirstOrDefault();
            
            foreach(var r in permis0.Roles)
            {   
               if (myRoles.Where(x => r.Contains(x.RoleName.ToLower().ToString())).FirstOrDefault() != null)
                {
                    return true;
                }
            }

            return false;


            
        }


        public void WriteLog(string path, ActionExecutingContext filterContext)
        {
            Model.Sys_Log logEntity = new Model.Sys_Log();
            logEntity.CategoryType = 2;
            logEntity.OperateTypeId = ((int)OperationType.Visit);
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Visit);
            logEntity.OperateAccount = LoticLight.Session.WebSession.CurrentUser.UserName;
            logEntity.OperateUserId = LoticLight.Session.WebSession.CurrentUser.Id;
            Model.Sys_Menu menu =new Sys_Menu() ;
            var action=Business.Sys_ActionService.Instance.LoadEntitiesNoTracking(o => o.ActionHref.Contains(path.Trim())).FirstOrDefault();

            if (action == null)
                menu = Business.Sys_MenuService.Instance.LoadEntitiesNoTracking(o => o.MenuHref.Contains(path.Trim())).FirstOrDefault();
            else
                menu = Business.Sys_MenuService.Instance.FindEntities(action.MenuId);

            string MenuId =menu==null?"0":menu.Id;
            string MenuName = menu==null?"其他":menu.MenuName;
            if (action != null) MenuName += "-" + action.ActionName;

            logEntity.MenuId = MenuId;
            logEntity.MenuName = MenuName;
            logEntity.AccessURL = path;
            logEntity.ExecuteResult = "1";
            logEntity.ExecuteResultJson = "访问地址：" + path;
            Business.Sys_LogService.WriteLog(logEntity);

            ///log4 暂时不开启，占io
            var log = LogFactory.GetLogger("InfoLog");
            LogMessage logMessage = new LogMessage();
            logMessage.OperationTime = DateTime.Now;
            logMessage.Url = path;
            logMessage.Class = filterContext.Controller.ToString();
            logMessage.Ip = LoticLight.Utility.Net.Ip;
            logMessage.Host = Net.Host;
            logMessage.Browser = Net.Browser;
            logMessage.UserName = LoticLight.Session.WebSession.CurrentUser.UserName + "|" + LoticLight.Session.WebSession.CurrentUser.Id;
            string strMessage = new LogFormat().InfoFormat(logMessage);
            log.Info(strMessage);


        }



        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        public bool checkPermissions(string path)
        {
            var initdata = LoticLight.Session.WebSession.CurrentAccount.initdata;

            if (initdata == null) return false;
            if (initdata.menus.Count(o => o.UrlAddress == path.Trim()) > 0)
                return true;
            foreach (var item in initdata.menusActions)
            {
                List<Model.Sys_Action> actions = (List<Model.Sys_Action>)item.Value;
                if (actions.Count(o => o.ActionHref.Trim() == path.Trim()) > 0)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 判断是否登录
        /// </summary>
        protected bool checkLogin()
        {
            var user = LoticLight.Session.WebSession.CurrentUser;
            if (user == null)
            {                
                return false;
            }
            return true;
        }
        
        #endregion

        #region 信息返回

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonNewResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,

            };
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message }.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = data }.ToJson());
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
        }

        //protected virtual JsonResult YznOK(string message)
        //{
        //    var res = new JsonResult();
        //    res.Data = new AjaxResult { type = ResultType.success, message = message };

        //    res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return res;
        //}

        //protected virtual JsonResult YznError(string message)
        //{
        //    var res = new JsonResult();
        //    res.Data = new AjaxResult { type = ResultType.error, message = message };

        //    res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return res;
        //}


        #endregion


        #region GZ权限
        public class PermissionAttribute : FilterAttribute, IActionFilter
        {
            public List<EnumBusinessPermission> Permissions { get; set; }

            public PermissionAttribute(params EnumBusinessPermission[] parameters)
            {
                //验证权限
                //foreach (var para in parameters)
                //{
                //    if (!SysHelper.IsInFunc(para)) {
                //        //return Controller.RedirectToAction("Info", "Dispatch", new { msg = "对不起，您没有该功能权限，请与系统管理员联系！" });
                //       throw new Exception("对不起，您没有该功能权限，请与系统管理员联系！");
                //    }
                //}
                Permissions = parameters.ToList();
            }

            public PermissionAttribute(string controller = "", string view = "", string func = "")
            {

            }


            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
                //throw new NotImplementedException();
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                //throw new NotImplementedException();
            }
        }
        public class NotCheckPermissionAttribute : FilterAttribute, IActionFilter
        {
            public List<EnumBusinessPermission> Permissions { get; set; }

            public NotCheckPermissionAttribute(params EnumBusinessPermission[] parameters)
            {
                //验证权限
                //foreach (var para in parameters)
                //{
                //    if (!SysHelper.IsInFunc(para)) {
                //        //return Controller.RedirectToAction("Info", "Dispatch", new { msg = "对不起，您没有该功能权限，请与系统管理员联系！" });
                //       throw new Exception("对不起，您没有该功能权限，请与系统管理员联系！");
                //    }
                //}
                Permissions = parameters.ToList();
            }

            public NotCheckPermissionAttribute(string controller = "", string view = "", string func = "")
            {

            }


            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
                //throw new NotImplementedException();
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                //throw new NotImplementedException();
            }
        }

        public class SimpleCheckPermissionAttribute : FilterAttribute, IActionFilter
        {
            public List<string> Roles { get; set; }

            public SimpleCheckPermissionAttribute(params string[] roles)
            {
                //验证权限
                //foreach (var para in parameters)
                //{
                //    if (!SysHelper.IsInFunc(para)) {
                //        //return Controller.RedirectToAction("Info", "Dispatch", new { msg = "对不起，您没有该功能权限，请与系统管理员联系！" });
                //       throw new Exception("对不起，您没有该功能权限，请与系统管理员联系！");
                //    }
                //}
                Roles = roles.ToList();
            }



            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
                //throw new NotImplementedException();
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                //throw new NotImplementedException();
            }
        }


        #endregion


        #region filedown

        [NotCheckPermission]
        public ActionResult ExportExcel(string sql,string toFile , string Title = "",System.Data.CommandType cmdType=System.Data.CommandType.Text, params System.Data.SqlClient.SqlParameter[] prams)
        {
            var ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnSting(), cmdType,
                 sql,prams);
            
            var ms = ExcelTool.DataTableToExcel.RenderDataTableToExcel(ds.Tables[0], Title);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", toFile);

        }
        [NotCheckPermission]
        public ActionResult ExportExcel(System.Data.DataTable dt, string toFile, string Title = "")
        {
            var ms = ExcelTool.DataTableToExcel.RenderDataTableToExcel(dt, Title);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", toFile);

        }
        #endregion



    }
}
