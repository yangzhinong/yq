using LoticLight.Model;
using LoticLight.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business
{
    public abstract class BaseService<T> where T : class,new()
    {

        public BaseRepository<T> CurrentRepository { get; set; }
        public Repository.ReporsitorySession _dbSession = ReporsitorySessionnFactory.GetCurrentDbSession();

        //基类的构造函数
        public BaseService()
        {
            SetCurrentRepository();  //构造函数里面调用了此设置当前仓储的抽象方法
        }

        //构造方法实现赋值 
        public abstract void SetCurrentRepository();  //约束子类必须实现这个抽象方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntities(T entity)
        {
            var addentity = CurrentRepository.AddEntities(entity);
            _dbSession.SaveChanges();
            return addentity;
        }
        /// <summary>
        /// 添加；不含常用字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntitiesOut(T entity)
        {
            var addentity = CurrentRepository.AddEntitiesOut(entity);
            _dbSession.SaveChanges();
            return addentity;
        }
        /// <summary>
        /// 添加；不含常用字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntitiesNos(T entity)
        {
            var addentity = CurrentRepository.AddEntitiesNos(entity);
            _dbSession.SaveChanges();
            return addentity;
        }
        /// <summary>
        /// 添加,不提交事物
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntitiesNo(T entity)
        {
            var addentity = CurrentRepository.AddEntities(entity);
            //_dbSession.SaveChanges();
            return addentity;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<T> AddEntities(List<T> entity)
        {//如果在这里操作多个表的话，实现批量的操作
            //调用T对应的仓储来添加
            foreach (var item in entity)
            {
                CurrentRepository.AddEntities(item);
            }
            _dbSession.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public bool UpdateEntities(T entity)
        {
            var updateEntity = CurrentRepository.UpdateEntities(entity);
            _dbSession.SaveChanges();
            return updateEntity;
        }
        /// <summary>
        /// 修改,不提交事物
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntitiesNo(T entity)
        {
            var updateEntity = CurrentRepository.UpdateEntities(entity);
            //_dbSession.SaveChanges();
            return updateEntity;
        }
        /// <summary>
        ///删除（逻辑删除）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntities(T entity)
        {
            var deleteEntity = CurrentRepository.DeleteEntities(entity);
            _dbSession.SaveChanges();
            return deleteEntity;
        }
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<T> entity)
        {
            var deleteEntity = CurrentRepository.DeleteEntities(entity);
            _dbSession.SaveChanges();
            return deleteEntity;
        }

        /// <summary>
        /// 移除（物理删除）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool RemoveEntities(T entity)
        {
            var deleteEntity = CurrentRepository.RemoveEntities(entity);
            _dbSession.SaveChanges();
            return deleteEntity;

        }
        /// <summary>
        /// 移除（物理删除）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool RemoveEntities(List<T> entity)
        {
            var deleteEntity = CurrentRepository.RemoveEntities(entity);
            _dbSession.SaveChanges();
            return deleteEntity;
        }


        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> wherelambda)
        {
            return CurrentRepository.LoadEntities(wherelambda);
        }
        /// <summary>
        /// 无条件查询
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>

        public IQueryable<T> LoadEntities()
        {
            return CurrentRepository.LoadEntities();
        }

        /// <summary>
        /// 条件查询（NoTracking）
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>

        public IQueryable<T> LoadEntitiesNoTracking(Expression<Func<T, bool>> wherelambda)
        {
            return CurrentRepository.LoadEntitiesNoTracking(wherelambda);
        }
        /// <summary>
        /// 无条件查询（NoTracking）
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>

        public IQueryable<T> LoadEntitiesNoTracking()
        {
            return CurrentRepository.LoadEntitiesNoTracking();
        }



        /// <summary>
        /// 根据主键获取实体对象
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>

        public T FindEntities(params object[] keyValues)
        {
            return CurrentRepository.Find(keyValues);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>

        public IList<T> LoadPagerEntities(Expression<Func<T, bool>> condition,
            int pageIndex,
            int pageSize,
            out int total,
            params LoticLight.Model.OrderModelField[] orderByExpression)
        {

            return CurrentRepository.LoadPagerEntities(condition, pageIndex, pageSize, out total, orderByExpression);
        }


    }
}
