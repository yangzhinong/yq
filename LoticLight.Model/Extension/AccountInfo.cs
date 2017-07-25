using System;
using System.Collections.Generic;
using System.Linq;

namespace LoticLight.Model
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class AccountInfo
    {
      
        /// <summary>
        /// 登录令牌
        /// </summary>
        public string Token { get; set; }


        /// <summary>
        /// 登录用户
        /// </summary>
        public Model.Sys_User User { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 访问的IP地址
        /// </summary>
        public string HostIPAddress { get; set; }

        /// <summary>
        /// 最近检查时间
        /// </summary>
        public DateTime LastCheckTime { get; set; }

        /// <summary>
        /// Session存在总时长
        /// </summary>
        public int SessionTimeOut { get; set; }

        public int TimeCount { get; set; }

        /// <summary>
        /// 当前有效时长
        /// </summary>
        public int LeftTimeCount { get; set; }

        public Model.initData initdata { get; set; }
    }
}