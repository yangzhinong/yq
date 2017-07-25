using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Linq.Dynamic;

namespace LoticLight.Web
{
    public class JQGridHelper<T>
    {

        public List<T> rows { get; set; }

        public int total { get; set; }

        public int page { get; set; }

        public int records { get; set; }


        public static JQGridHelper<T> GetPageData(IQueryable<T> list, LoticLight.Model.Pagination pagination ){
           
            
            var ret = new JQGridHelper<T>();

            //list = GetNotDeleteData(list);
            ret.total = list.Count();

            
            if (ret.total>0  &&  !string.IsNullOrEmpty(pagination.sidx))
            {
                var oFirst = list.First();
                var oType = oFirst.GetType();
                System.Reflection.PropertyInfo[] myPropertyInfo = oType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                var qry = (from p in myPropertyInfo
                           where p.Name.ToLower() == pagination.sidx.ToLower()
                           select p.Name).ToList();

                if (qry.Count > 0)
                {
                    list = list.OrderBy(qry[0] + " " + pagination.sord.ToUpper());
                }
            }
            else
            {
                list = list.OrderBy("Id Desc");
            }
            ret.page = pagination.page;
            ret.rows = list.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
            return ret;

        }

        /// <summary>
        /// 得到未标记删除的数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static IQueryable<T> GetNotDeleteData(IQueryable<T> list)
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
                list=list.Where(lambda);
            }
            return list ;
        }


        /// <summary>
        /// 动态派寻
        /// </summary>
        /// <param name="orderByExpression"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private static IQueryable<T> DynamicOrder( IQueryable<T> query, params OrderModelField[] orderByExpression)
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
    }
}