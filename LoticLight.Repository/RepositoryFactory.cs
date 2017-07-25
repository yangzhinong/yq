using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public partial class RepositoryFactory
    {

        /// <summary>
        /// 帮我们返回当前线程内的数据库上下文，如果当前线程内没有上下文，那么创建一个上下文，并保证
        /// 上下文是实例在线程内部唯一 
        /// 在EF4.0以前使用ObjectsContext对象
        /// </summary>
        /// <returns></returns>
        public static DbContext GetCurrentDbContext()
        {
            //当第二次执行的时候直接取出线程嘈里面的对象
            //CallContext:是线程内部唯一的独用的数据槽(一块内存空间)
            //数据存储在线程栈中
            //线程内共享一个单例
            DbContext dbcontext = CallContext.GetData("DbContext") as DbContext;

            //判断线程里面是否有数据
            if (dbcontext == null)  //线程的数据槽里面没有次上下文
            {
                dbcontext = new Entities();  //创建了一个EF上下文
                //存储指定对象
                CallContext.SetData("DbContext", dbcontext);
            }
            return dbcontext;
        }
    }
}
