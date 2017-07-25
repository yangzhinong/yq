using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Model
{
    public class SystemSetting
    {
           /// <summary>
           /// 浏览器session过期时间
           /// </summary>
           public static int SessionOut
           {
               get{return GetSessionOut();}
           }
            /// <summary>
            /// 系统登录过期时间
            /// </summary>
           public static int AccountOut 
           {
               get { return GetAccountOut(); }
           }
           /// <summary>
           /// 令牌索引
           /// </summary>
          public static string  Token = "AccountInfo_Token";

       private static int _sessionOut=-1;
       private static int _accountOut = -1;
        /// <summary>
        /// 得到浏览器session过期时间
        /// </summary>
       private static int GetSessionOut()
        {
            if (_sessionOut != -1) return _sessionOut;
           var obj =new Model.Entities().Sms_Setting.FirstOrDefault(o => o.Code == "SessionOut");
           _sessionOut = int.Parse(obj.Value.Trim());
           return _sessionOut;
        }

        /// <summary>
        /// 得到系统登录过期时间
        /// </summary>
       private static int GetAccountOut()
        {
            if (_accountOut != -1) return _accountOut;
            var obj = new Model.Entities().Sms_Setting.FirstOrDefault(o => o.Code == "AccountOut");
            _accountOut = int.Parse(obj.Value.Trim());
            return _accountOut;
        }

    }
}
