
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public class ReporsitorySessionnFactory
    {

        /// <summary>
        /// 保证了线程内DbSession实例唯一
        /// </summary>
        /// <returns></returns>
        public static ReporsitorySession GetCurrentDbSession()
        {
            ReporsitorySession _dbSession = CallContext.GetData("DbSession") as ReporsitorySession;
            if (_dbSession == null)
            {
                _dbSession = new ReporsitorySession();
                CallContext.SetData("DbSession", _dbSession);
            }
            return _dbSession;
        }
    }
}
