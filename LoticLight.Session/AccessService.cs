using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoticLight.Session
{

    /// <summary>
    /// LG自定义Session操作类，保存全局登录信息
    /// </summary>
    public class AccessService
    {
       
        /// <summary>
        /// 超时待移除Session列表
        /// </summary>
        private ArrayList _RemoveSesions = new ArrayList();
        /// <summary>
        /// 检查Sesion超时的线程
        /// </summary>
        private Thread _RunTimeThread;
        /// <summary>
        /// 哈希表保存自定义Session列表
        /// </summary>
        private Hashtable _Sessions = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// 检查Sesion超时的线程状态
        /// </summary>
        private bool m_IsStated = false;
        /// <summary>
        /// 单利模式全局Sesion
        /// </summary>
        private static AccessService ServerInstance;

        

        /// <summary>
        /// 构造方法
        /// </summary>
        private AccessService()
        {
            this._RunTimeThread = new Thread(new ThreadStart(this.ServerRunTime));
            StartServer();
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="sc"></param>
        public void AddSessionContext(SessionContext sc)
        {
            this._Sessions[sc.UserToken] = sc;
           
        }

        /// <summary>
        /// 移除所有Session
        /// </summary>
        public void ClearSession()
        {
            this._Sessions.Clear();
        }

        /// <summary>
        /// 得到缓存对象
        /// </summary>
        /// <param name="UserToken">哈希表索引(用户令牌)</param>
        /// <returns></returns>
        public SessionContext GetSessionContext(string UserToken)
        {
            if ((UserToken == null) || (UserToken == ""))
            {
                return null;
            }
            if (!this._Sessions.Contains(UserToken))
            {
                return null;
            }
            return (SessionContext)this._Sessions[UserToken];
        }
        /// <summary>
        /// 得到当前Session个数
        /// </summary>
        /// <returns></returns>
        public int GetSessionCount()
        {
            return this._Sessions.Count;
        }
       
       
        /// <summary>
        /// 返回当前全部Session列表
        /// </summary>
        /// <returns>Session列表</returns>
        public List<SessionContext> GetSessionList()
        {
            List<SessionContext> contexts = new List<SessionContext>();

            foreach (DictionaryEntry entry in this._Sessions)
            {

                SessionContext context = (SessionContext)entry.Value;
                contexts.Add(context);


            }
            return contexts;
        }
        /// <summary>
        /// 验证用户是否登录
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public bool ExistsLogin(string UserId)
        {
            bool b = false;
            SessionContext obj = GetSessionContextByUserId(UserId);

            if (obj != null)
                b = true;
            return b;

        }

        /// <summary>
        /// 得到Session
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public SessionContext GetSessionContextByUserId(string UserId)
        {
            return GetSessionList().Find(p => p.Account.User.Id == UserId);

        }
        /// <summary>
        /// 得到所有登录用户的信息
        /// </summary>
        /// <returns></returns>
        public List<Model.AccountInfo> GetOnlineUserList()
        {
            List<SessionContext> listSession = GetSessionList();
            List<Model.AccountInfo> list = new List<Model.AccountInfo>();
            int i = 0;
            foreach (SessionContext s in listSession)
            {
                Model.AccountInfo obj = new Model.AccountInfo();
                obj = s.Account;
                obj.BeginTime=s.BeginTime;
                obj.HostIPAddress=s.HostIPAddress;
                obj.LastCheckTime=s.LastCheckTime;
                obj.LeftTimeCount=s.LeftTimeCount;
                obj.SessionTimeOut=s.SessionTimeOut;
                obj.TimeCount=s.TimeCount;
                list.Add(obj);
                i++;
            }
            return list;

        }

        /// <summary>
        /// 实例，单例模式
        /// </summary>
        public static AccessService Instance
        {
            get
            {
                if (ServerInstance == null)
                {
                    ServerInstance = new AccessService();
                }
                return ServerInstance;
            }
        }

        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="UserToken">用户令牌</param>
        /// <returns></returns>
        public bool RemoveSessionContext(string UserToken)
        {
            bool b = true;
            this._Sessions.Remove(UserToken);
            return b;
        }

        /// <summary>
        /// 重启Session线程
        /// </summary>
        public void ResetServer()
        {
            this.StopServer();
            this.StartServer();
            
        }

        /// <summary>
        /// Session线程方法，判断Session到期时间
        /// </summary>
        private void ServerRunTime()
        {
            while (this.m_IsStated)
            {
                try
                {
                    foreach (DictionaryEntry entry in this._Sessions)
                    {
                        SessionContext context = (SessionContext)entry.Value;
                        if (context.LeftTimeCount <= 0)
                        {
                            this._RemoveSesions.Add(context.UserToken);
                        }
                        Thread.Sleep(10);
                    }
                    int count = this._RemoveSesions.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            string key = this._RemoveSesions[i].ToString();
                            this._Sessions.Remove(key);
                        }
                    }
                    this._RemoveSesions.Clear();
                }
                catch (Exception)
                {
                    this._RemoveSesions.Clear();
                }
                Thread.Sleep(1000);//现场每秒执行一次
            }
        }

        /// <summary>
        /// Session线程开始方法
        /// </summary>
        public void StartServer()
        {
            this.m_IsStated = true;
            this._RunTimeThread.Start();
        }

        /// <summary>
        /// Session线程结束方法
        /// </summary>
        public void StopServer()
        {
            this.m_IsStated = false;
            this._RunTimeThread.Abort();
            this._Sessions.Clear();
            this._RemoveSesions.Clear();
        }

        /// <summary>
        /// 当前Session线程运行状态
        /// </summary>
        public bool IsServerStarted
        {
            get
            {
                return this.m_IsStated;
            }
        }
    }
}
