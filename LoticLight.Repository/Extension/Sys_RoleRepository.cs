using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public partial class Sys_RoleRepository
    {
        /// <summary>
        /// 得到用户角色
        /// </summary>
        /// <returns></returns>
        public static List<Model.Sys_Role> GetRoleByUserId(string UserId)
        {
            var data = from r in Instance.LoadEntities()
                       from ur in Sys_RelationRepository.Instance.LoadEntitiesNoTracking()
                       where r.Id == ur.RelationTwo
                       && ur.RelationOne == UserId
                       && ur.RelationName == "User-Role"
                       && ur.IsDelete == false
                       && r.IsDelete == false
                       select r;
            return data.ToList();
 
        }
    }
}
