using LoticLight.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business
{
    public partial class Sys_LogService
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public static void WriteLog(Model.Sys_Log logEntity)
        {
            try
            {
               
                logEntity.OperateTime = DateTime.Now;
                logEntity.IPAddress = Net.Ip;
                logEntity.Host = Net.Host;
                logEntity.Browser = Net.Browser;
              Instance.AddEntities(logEntity);
             
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
