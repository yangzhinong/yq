using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business
{
    public partial class Sys_GridViewService
    {

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        public static List<Model.Sys_GridView> GetViewsByUserId(string Id)
        {
            return Repository.Sys_GridViewRepository.GetViewsByUserId(Id);
        }
        public static Dictionary<string, object> GetMenuView(string Id, List<Model.Menus> menus)
        {
            Dictionary<string, object> mvs = new Dictionary<string, object>();
            var action = GetViewsByUserId(Id);
            foreach (var menu in menus)
            {
                var views = action.FindAll(o => o.MenuId == menu.ModuleId);
                if (mvs.Count(o => o.Key == menu.ModuleId) == 0)
                mvs.Add(menu.ModuleId, views);
            }
            return mvs;

        }


        /// <summary>
        /// 取得角色的所有视图列表
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static List<Model.Sys_GridView> GetRoleViews(string RoleId)
        {
            return Repository.Sys_GridViewRepository.GetRoleViews(RoleId);
        }
    }
}
