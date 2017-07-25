using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business
{
    public partial class Sys_ActionService
    {
        /// <summary>
        /// 获取操作列表
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        public static List<Model.Sys_Action> GetActionsByUserId(string Id)
        {
            return Repository.Sys_ActionRepository.GetActionsByUserId(Id);
        }
        public static Dictionary<string, object> GetMenuActin(string Id, List<Model.Menus> menus)
        {
            Dictionary<string, object> mas = new Dictionary<string, object>();
            var action = GetActionsByUserId(Id);
            foreach (var menu in menus)
            {
                var actions = action.FindAll(o => o.MenuId == menu.ModuleId);
                if(mas.Count(o=>o.Key==menu.ModuleId)==0)
                mas.Add(menu.ModuleId, actions);
            }
            return mas;
 
        }

        /// <summary>
        /// 取得角色的所有操作列表
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static List<Model.Sys_Action> GetRoleActions(string RoleId)
        {
            return Repository.Sys_ActionRepository.GetRoleActions(RoleId);
        }
    }
}
