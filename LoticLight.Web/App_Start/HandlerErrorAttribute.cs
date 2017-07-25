using LoticLight.Log;
using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoticLight.Utility;
using LoticLight.Business;

namespace LoticLight.Web
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 控制器方法中出现异常，会调用该方法捕获异常
        /// </summary>
        /// <param name="context">提供使用</param>
        public override void OnException(ExceptionContext context)
        {
            WriteLog(context);
            base.OnException(context);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new ContentResult { Content = new AjaxResult { type = ResultType.error, message = context.Exception.Message }.ToJson() };
        }
        /// <summary>
        /// 写入日志（log4net）
        /// </summary>
        /// <param name="context">提供使用</param>
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            if (LoticLight.Session.WebSession.Current==null)
                return;
            var log = LogFactory.GetLogger("ErrorLog");
            Exception Error = context.Exception;
            LogMessage logMessage = new LogMessage();
            logMessage.OperationTime = DateTime.Now;
            logMessage.Url = HttpContext.Current.Request.RawUrl;
            logMessage.Class = context.Controller.ToString();
            logMessage.Ip = LoticLight.Utility.Net.Ip;
            logMessage.Host = Net.Host;
            logMessage.Browser = Net.Browser;
            logMessage.UserName = LoticLight.Session.WebSession.CurrentUser.UserName + "|" + LoticLight.Session.WebSession.CurrentUser.Id;
          
            if (Error.InnerException == null)
            {
                logMessage.ExceptionInfo = Error.ToString();
            }
            else
            {
                logMessage.ExceptionInfo = Error.InnerException.ToString();
            }
            string strMessage = new LogFormat().ExceptionFormat(logMessage);
            log.Error(strMessage);

            Model.Sys_Log logEntity = new  Model.Sys_Log();
            logEntity.CategoryType = 4;
            logEntity.OperateTypeId = ((int)OperationType.Exception);
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
            logEntity.OperateAccount = logMessage.UserName;
            logEntity.OperateUserId = LoticLight.Session.WebSession.CurrentUser.Id;
            logEntity.ExecuteResult = "-1";
            logEntity.ExecuteResultJson = strMessage;
            Sys_LogService.WriteLog(logEntity);
           
        }
       
    }
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandlerErrorAttribute());
        }
    }
}