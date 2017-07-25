using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business
{
    public partial class Sys_RoleService
    {
        /// <summary>
        /// 角色列表数据
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public static List<object> GetRoleListByUserId(string UserId)
        {
            var data = Repository.Sys_RoleRepository.GetRoleByUserId(UserId);
            var allRoledata = Repository.Sys_RoleRepository.Instance.LoadEntities();
            List<object> list = (from r in allRoledata
                                 select new
                                 {
                                     Id = r.Id,
                                     RoleName = r.RoleName,
                                     ischeck = data.Count(o => o.Id == r.Id) > 0 ? 1 : 0,
                                     Describe = r.Describe
                                 }).ToList<object>();
            return list;

        }

        public static List< Model.Sys_Role> GetRoleListByUserId2(string UserId)
        {
            var data = Repository.Sys_RoleRepository.GetRoleByUserId(UserId);
            var allRoledata = Repository.Sys_RoleRepository.Instance.LoadEntities();
            var list = (from r in allRoledata
                                 select new Model.Sys_Role
                                 {
                                     Id = r.Id,
                                     RoleName = r.RoleName
                                 }).ToList();
            return list;

        }

        public static void SaveUserRole(string userId, string roleIds)
        {
            string[] _roleIds = roleIds.Split(',');
            var allowRole = Repository.Sys_RoleRepository.GetRoleByUserId(userId);
            //删除
            foreach (var item in allowRole)
            {
                if (_roleIds.Count(o => ((string)o) == item.Id) == 0)
                {
                    var obj_r=Repository.Sys_RelationRepository.Instance.LoadEntitiesNoTracking(o=>o.RelationOne==userId&&o.RelationTwo==item.Id). FirstOrDefault();
                    Repository.Sys_RelationRepository.Instance.DeleteEntities(obj_r);
                }

            }
            //添加
            foreach (var item in _roleIds)
            {
                if (allowRole.Count(o => o.Id == item) == 0)
                {
                    Model.Sys_Relation obj = new Model.Sys_Relation()
                    {
                        RelationOne = userId,
                        RelationTwo = item,
                        RelationName = "User-Role"
                    };
                    Repository.Sys_RelationRepository.Instance.AddEntities(obj);
 
                }
            }

            Instance._dbSession.SaveChanges();
 
        }
    }
}
