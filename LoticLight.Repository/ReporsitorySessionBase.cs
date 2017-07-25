
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LoticLight.Repository
{

    /// <summary>
    /// 数据库交互会话，
    /// 如果操作数据库的话直接从这里来操作
    /// </summary>
    public partial class ReporsitorySession  //代表的是应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    {
        /// <summary>
        /// 代表当前应用程序跟数据库的回话内所有的实体变化，更新会数据库
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()  //UintWork单元工作模式
        {
            try
            {
                    return RepositoryFactory.GetCurrentDbContext().SaveChanges();
            }
            catch (DbEntityValidationException e )
            {
 
                throw e;
            }
            //调用EF上下文的SaveChanges的方法
          

        }
        public int ExcuteSql(string strSql, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

    }
}
