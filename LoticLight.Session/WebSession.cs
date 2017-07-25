using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoticLight.Session
{

    /// <summary>
    /// 网站登录Session信息
    /// </summary>
    public static class WebSession
    {
        /// <summary>
        /// 登录令牌索引值
        /// </summary>
        private static string Token = SystemSetting.Token;

        /// <summary>
        /// 当前Session信息
        /// </summary>
        public static SessionContext Current
        {
            get
            {
                try
                {
                    string token = HttpContext.Current.Session[Token] as string;
                    if (!string.IsNullOrEmpty(token))
                    {
                        return AccessService.Instance.GetSessionContext(token);
                    }
                }
                catch
                { }
                return null;
            }
        }
        /// <summary>
        /// 用户登录信息
        /// </summary>
        public static AccountInfo CurrentAccount
        {
            get
            {
               

                if (Current != null)
                    return Current.Account;
                else
                    return null;
            }
        }

        public static Model.Sys_User CurrentUser 
        {
            get
            {


                if (Current != null)
                    return Current.Account.User;
                else
                    return null;
            }
        }

       /// <summary>
        /// 设置当前Session上下文信息
       /// </summary>
       /// <param name="account"></param>
        public static void SetCurrentAccount(AccountInfo account)
        {
            if (true
                )
            {
                string token = Guid.NewGuid().ToString();
                HttpContext.Current.Session[Token] = token;
                HttpContext.Current.Session.Timeout = SystemSetting.SessionOut;
                SessionContext cxt = new SessionContext(account, token);
                AccessService.Instance.AddSessionContext(cxt);
            }

        }


   

      /// <summary>
      /// 移除Session
      /// </summary>
      /// <returns></returns>
        public static bool RemoveSession()
        {
            if (Current.UserToken != null) { 
            AccessService.Instance.RemoveSessionContext(Current.UserToken);
            HttpContext.Current.Session[Token] = null;
             }
            return true;
        }
    }
}
