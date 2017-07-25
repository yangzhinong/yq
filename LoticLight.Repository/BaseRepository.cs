
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LoticLight.Model;

namespace LoticLight.Repository
{
    public partial class BaseRepository<T> where T : class
    {

        //EF上下文的实例保证，线程内唯一
        //实例化EF框架
        //DataModelContainer db = new DataModelContainer();

        //获取的实当前线程内部的上下文实例，而且保证了线程内上下文实例唯一
        private DbContext db = RepositoryFactory.GetCurrentDbContext();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntities(T entity)
        {
            #region 新增记录时自动添加创建者、创建时间、修改者、修改时间
            if (typeof(T).GetProperty("Creator") != null)
                typeof(T).GetProperty("Creator").SetValue(entity, LoticLight.Session.WebSession.CurrentAccount.User.UserName + "|" + LoticLight.Session.WebSession.CurrentAccount.User.Id);
            if (typeof(T).GetProperty("CreatTime") != null)
                typeof(T).GetProperty("CreatTime").SetValue(entity, DateTime.Now);
            if (typeof(T).GetProperty("ModifyTime") != null)
                typeof(T).GetProperty("ModifyTime").SetValue(entity, DateTime.Now);
            if (typeof(T).GetProperty("Modifier") != null)
                typeof(T).GetProperty("Modifier").SetValue(entity, LoticLight.Session.WebSession.CurrentAccount.User.UserName + "|" + LoticLight.Session.WebSession.CurrentAccount.User.Id);
            if (typeof(T).GetProperty("Id") != null)
                typeof(T).GetProperty("Id").SetValue(entity, Guid.NewGuid().ToString());
            #endregion
            db.Entry<T>(entity).State = EntityState.Added;
            //db.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntitiesOut(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Added;
            //db.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntitiesNos(T entity)
        {
            if (typeof(T).GetProperty("Id") != null)
                typeof(T).GetProperty("Id").SetValue(entity, Guid.NewGuid().ToString());
            db.Entry<T>(entity).State = EntityState.Added;
            //db.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<T> AddEntities(List<T> entity)
        {
            foreach (var item in entity)
            {
                AddEntities(item);
            }
            return entity;
        }


        /// <summary>
        /// 通过主键获取实例
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            return db.Set<T>().Find(keyValues);
        }  
        //修改
        public bool UpdateEntities(T entity)
        {
            if (typeof(T).GetProperty("ModifyTime") != null)
                typeof(T).GetProperty("ModifyTime").SetValue(entity, DateTime.Now);
            if (typeof(T).GetProperty("Modifier") != null)
                typeof(T).GetProperty("Modifier").SetValue(entity,  LoticLight.Session.WebSession.CurrentAccount.User.UserName+ "|" +LoticLight.Session.WebSession.CurrentAccount.User.Id );
            if (db.Entry<T>(entity).State == EntityState.Modified)
            {
                return true;
            }
            else if (db.Entry<T>(entity).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(entity);
                    db.Entry<T>(entity).State = EntityState.Modified;
                }
                catch (InvalidOperationException e)
                {
                    throw e;
                    //string ID="";
                    //if (typeof(T).GetProperty("ID") != null)
                    //    ID = (string)typeof(T).GetProperty("ID").GetValue(entity);
                    //T old = Find(ID);
                    //db.Entry(old).CurrentValues.SetValues(entity);
                }
                return true;
            }
      
            return true;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntities(T entity)
        {
            //if (typeof(T).GetProperty("ModifyTime") != null)
            //    typeof(T).GetProperty("ModifyTime").SetValue(entity, DateTime.Now);
            //if (typeof(T).GetProperty("Modifier") != null)
            //    typeof(T).GetProperty("Modifier").SetValue(entity, LoticLight.Session.WebSession.CurrentAccount.User.UserName + "|" + LoticLight.Session.WebSession.CurrentAccount.User.Id);
            //if (typeof(T).GetProperty("IsDelete" )!= null)
            //    typeof(T).GetProperty("IsDelete").SetValue(entity, true);
            //if (db.Entry<T>(entity).State == EntityState.Modified)
            //{
            //    return true;
            //}
            //else
            //{
            //    db.Set<T>().Attach(entity);
            //    db.Entry<T>(entity).State = EntityState.Modified;
            //}

            if (typeof(T).GetProperty("IsDelete" )!= null)
               typeof(T).GetProperty("IsDelete").SetValue(entity, true);
            UpdateEntities(entity);
            
            return true;
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<T> entity)
        {
            foreach (var item in entity)
            {
                db.Set<T>().Attach(item);
            if (typeof(T).GetProperty("IsDelete") != null)
                typeof(T).GetProperty("IsDelete").SetValue(item, true);
            }
           
            return true;
        }

        //移除（物理删除）
        public bool RemoveEntities(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            return true;
        }
        /// <summary>
        /// 移除（物理删除）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool RemoveEntities(List<T> entity)
        {
            foreach (var item in entity)
            {
                db.Set<T>().Attach(item);
                db.Entry<T>(item).State = EntityState.Deleted;
            }

            return true;
        }
        

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities( Expression<Func<T, bool>> wherelambda)
        {
            var tempData = db.Set<T>().Where<T>(wherelambda);
            ///只显示标记为【未删除】的数据
            if (typeof(T).GetProperty("IsDelete") != null)
            {

                var parameter = Expression.Parameter(typeof(T), "u");
                //创建参数 IsDelete

                var par1 = Expression.PropertyOrField(parameter, "IsDelete");

                //创建常数 false为未删除状态

                var constant = Expression.Constant(false);

                //创建 IsDelete==false

                var query = Expression.Equal(par1, constant);

                //获取Lambda表达式

                var lambda = Expression.Lambda<Func<T, Boolean>>(query, parameter);
                tempData = tempData.Where<T>(lambda.Compile()).AsQueryable();

            }
            return tempData.AsQueryable();
        }

        /// <summary>
        /// 无条件查询所有数据
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities()
        {
            var tempData = db.Set<T>().ToList().AsQueryable();
            ///只显示标记为【未删除】的数据
            if (typeof(T).GetProperty("IsDelete") != null)
            {

                var parameter = Expression.Parameter(typeof(T), "u");
                //创建参数 IsDelete

                var par1 = Expression.PropertyOrField(parameter, "IsDelete");

                //创建常数 false为未删除状态

                var constant = Expression.Constant(false);

                //创建 IsDelete==false

                var query = Expression.Equal(par1, constant);

                //获取Lambda表达式

                var lambda = Expression.Lambda<Func<T, Boolean>>(query, parameter);
                tempData = tempData.Where<T>(lambda.Compile()).AsQueryable();

            }
            return tempData.AsQueryable();
        }


        /// <summary>
        /// 条件查询(NoTracking)
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntitiesNoTracking(Expression<Func<T, bool>> wherelambda)
        {
            var tempData = db.Set<T>().AsNoTracking().Where<T>(wherelambda);
            ///只显示标记为【未删除】的数据
            if (typeof(T).GetProperty("IsDelete") != null)
            {

                var parameter = Expression.Parameter(typeof(T), "u");
                //创建参数 IsDelete

                var par1 = Expression.PropertyOrField(parameter, "IsDelete");

                //创建常数 false为未删除状态

                var constant = Expression.Constant(false);

                //创建 IsDelete==false

                var query = Expression.Equal(par1, constant);

                //获取Lambda表达式

                var lambda = Expression.Lambda<Func<T, Boolean>>(query, parameter);
                tempData = tempData.Where<T>(lambda.Compile()).AsQueryable();

            }
            return tempData.AsQueryable().AsNoTracking();
        }

        /// <summary>
        /// 无条件查询(NoTracking)
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntitiesNoTracking()
        {
            var tempData = db.Set<T>().AsNoTracking().ToList().AsQueryable();
            ///只显示标记为【未删除】的数据
            if (typeof(T).GetProperty("IsDelete") != null)
            {

                var parameter = Expression.Parameter(typeof(T), "u");
                //创建参数 IsDelete

                var par1 = Expression.PropertyOrField(parameter, "IsDelete");

                //创建常数 false为未删除状态

                var constant = Expression.Constant(false);

                //创建 IsDelete==false

                var query = Expression.Equal(par1, constant);

                //获取Lambda表达式

                var lambda = Expression.Lambda<Func<T, Boolean>>(query, parameter);
                tempData = tempData.Where<T>(lambda.Compile()).AsQueryable();

            }
            return tempData.AsQueryable().AsNoTracking();
        }





        ////分页
        //public IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex, out int total,
        //    Expression<Func<T, bool>> whereLambda, bool isAsc, Func<T,S> orderByLambda)
        //{

        //    IQueryable<T> tempData ;

        //    tempData = db.Set<T>().Where<T>(whereLambda).AsQueryable();

        //    ///只显示标记为【未删除】的数据
        //    if (typeof(T).GetProperty("IsDelete") != null)
        //    {

        //        var parameter = Expression.Parameter(typeof(T), "u");
        //        //创建参数 IsDelete

        //        var par1 = Expression.PropertyOrField(parameter, "IsDelete");

        //        //创建常数 false为未删除状态

        //        var constant = Expression.Constant(false);

        //        //创建 IsDelete==true

        //        var query = Expression.Equal(par1, constant);

        //        //获取Lambda表达式

        //        var lambda = Expression.Lambda<Func<T, Boolean>>(query, parameter);
        //        whereLambda = lambda.And(whereLambda);
 
               
        //       // tempData = tempData.Where<T>(lambda.Compile()).AsQueryable();

        //    }
        //    total = db.Set<T>().Count(whereLambda);   
        //    //排序获取当前页的数据
        //    if (isAsc)
        //    {
        //        tempData = db.Set<T>().Where<T>(whereLambda).OrderBy<T, S>(orderByLambda).
        //              Skip<T>(pageSize * (pageIndex - 1)).
        //              Take<T>(pageSize)
        //              .AsQueryable();
        //    }
        //    else
        //    {
        //        tempData = db.Set<T>().Where<T>(whereLambda).OrderByDescending<T, S>(orderByLambda).
        //             Skip<T>(pageSize * (pageIndex - 1)).
        //             Take<T>(pageSize).AsQueryable();
        //    }

        //    return tempData.AsQueryable();
        //}


     /// <summary>
     /// 分页
     /// </summary>
     /// <param name="condition">查询条件</param>
     /// <param name="pageIndex">当前页</param>
     /// <param name="pageSize">每页大小</param>
     /// <param name="total">总数</param>
     /// <param name="orderByExpression">派寻字段</param>
     /// <returns></returns>
        public IList<T> LoadPagerEntities(
            Expression<Func<T, bool>> condition, 
            int pageIndex, 
            int pageSize, 
            out int total,
            params LoticLight.Model.OrderModelField[] orderByExpression)
        {
             ///只显示标记为【未删除】的数据
            condition = GetNotDeleteData(condition);
            //统计总数
            total = db.Set<T>().Count(condition);
            //构建查询语句
            var query = db.Set<T>().Where<T>(condition).AsQueryable();
            //动态排序
            query = DynamicOrder(orderByExpression, query);
            ///数据分页查询
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 动态派寻
        /// </summary>
        /// <param name="orderByExpression"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static IQueryable<T> DynamicOrder(OrderModelField[] orderByExpression, IQueryable<T> query)
        {

            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");

            if (orderByExpression != null && orderByExpression.Length > 0)
            {
                for (int i = 0; i < orderByExpression.Length; i++)
                {
                    //根据属性名获取属性
                    var property = typeof(T).GetProperty(orderByExpression[i].propertyName);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);

                    string OrderName = "";
                    if (i > 0)
                    {
                        OrderName = orderByExpression[i].IsDESC ? "ThenByDescending" : "ThenBy";
                    }
                    else
                        OrderName = orderByExpression[i].IsDESC ? "OrderByDescending" : "OrderBy";


                    MethodCallExpression resultExp =
                   Expression.Call(typeof(Queryable),
                   OrderName,
                   new Type[] { typeof(T),
                   property.PropertyType },
                   query.Expression,
                   Expression.Quote(orderByExp));
                    query = query.Provider.CreateQuery<T>(resultExp);
                }
            }
            return query;
        }

        /// <summary>
        /// 得到未标记删除的数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> GetNotDeleteData(Expression<Func<T, bool>> condition)
        {
            if (typeof(T).GetProperty("IsDelete") != null)
            {
                var parameterDelete = Expression.Parameter(typeof(T), "u");
                //创建参数 IsDelete

                var par1 = Expression.PropertyOrField(parameterDelete, "IsDelete");

                //创建常数 false为未删除状态

                var constant = Expression.Constant(false);

                //创建 IsDelete==true

                var isdelete = Expression.Equal(par1, constant);

                //获取Lambda表达式

                var lambda = Expression.Lambda<Func<T, Boolean>>(isdelete, parameterDelete);
                ///lambda条件拼接
                condition = lambda.And(condition);
            }
            return condition;
        }





    }
}
