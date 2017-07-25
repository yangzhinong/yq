using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoticLight.Session
{
    public class SessionContext
    {

        private DateTime m_BeginTime;//登录时间
        private string m_HostIPAddress = "";//IP地址
        private DateTime m_LastCheckTime;//最近一次访问时间
        public int m_SessionTimeOut;//Session会话丢失时间
        private AccountInfo m_Account;//登录用户信息
        private string m_UserToken;//登录用户令牌

        // 构造方法
        public SessionContext(AccountInfo account, string token)
        {
            
            this.m_Account = account;
            m_Account.Token = token;
            m_UserToken = token;
            this.m_SessionTimeOut = SystemSetting.AccountOut;//Session会话丢失时间(分钟)
            this.m_BeginTime = DateTime.Now;//当前登录时间
            this.m_LastCheckTime = this.m_BeginTime;//最近一次访问时间
            m_HostIPAddress = HttpContext.Current.Request.UserHostAddress; 
        }
        /// <summary>
        /// 每次访问时，更新最近访问时间，
        /// </summary>
        public void CheckSession()
        {
            this.m_LastCheckTime = DateTime.Now;
        }

        // 开始访问时间
        public DateTime BeginTime
        {
            get
            {
                return this.m_BeginTime;
            }
        }


        /// <summary>
        /// 当前登录信息
        /// </summary>
        public AccountInfo Account
        {
            get
            {
                return this.m_Account;
            }
        }
        /// <summary>
        /// 登录的Ip地址
        /// </summary>
        public string HostIPAddress
        {
            get
            {
                return this.m_HostIPAddress;
            }
            set
            {
                this.m_HostIPAddress = value;
            }
        }
        /// <summary>
        /// 最近检查访问时间
        /// </summary>
        public DateTime LastCheckTime
        {
            get
            {
                return this.m_LastCheckTime;
            }
        }
        /// <summary>
        /// 会话结束时长
        /// 分钟数
        /// </summary>
        public int LeftTimeCount
        {
            get
            {
                TimeSpan span = DateTime.Now.Subtract(this.m_LastCheckTime);
                return (this.m_SessionTimeOut - ((int)span.TotalMinutes));
            }
        }
        /// <summary>
        /// 会话时长
        /// </summary>
        public int SessionTimeOut
        {
            get
            {
                return this.m_SessionTimeOut;
            }
        }
        /// <summary>
        /// 登录时长，分钟数
        /// </summary>
        public int TimeCount
        {
            get
            {
                return (int)DateTime.Now.Subtract(this.m_BeginTime).TotalMinutes;
            }
        }


        /// <summary>
        /// 用户的登录令牌
        /// </summary>
        public string UserToken
        {
            get
            {
                return this.m_UserToken;
            }
        }

     
       

    }

    
}
