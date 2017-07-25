using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public partial class Sys_MenuRepository
    {
       private static Model.Entities db = (Entities)RepositoryFactory.GetCurrentDbContext();
        public static List<Model.Menus> GetmenusByUserId(string Id)
        {
            List<Model.Menus> menus =
                       (from ur in db.Sys_Relation
                        from r in db.Sys_Role
                        from rm in db.Sys_Relation
                        from m in db.Sys_Menu
                        where ur.RelationOne == Id
                        && r.Id == ur.RelationTwo
                        && r.Id == rm.RelationOne
                        && rm.RelationTwo == m.Id
                        && ur.RelationName == "User-Role"
                        && rm.RelationName == "Role-Menu"
                        && m.IsDelete==false
                        && r.IsDelete==false
                        && ur.IsDelete==false
                        && rm.IsDelete==false
                        select
                            new Menus
                            {
                                ModuleId = m.Id,
                                ParentId = m.ParentId,
                                EnCode = m.MenuCode,
                                FullName = m.MenuName,
                                Icon = m.MenuIcon,
                                UrlAddress = m.MenuHref,
                                IsLeaf = m.IsLeaf,
                                SortCode = m.SortCode.ToString()
                            }).OrderBy(o => o.SortCode).ToList();
            //去重
            menus = menus.Where((x, y) => menus.FindIndex(o => o.ModuleId == x.ModuleId) == y).ToList();
            return menus;

        }

        public static List<Model.Sys_Menu> GetmenusByRoleId(string RoleId)
        {
            List<Model.Sys_Menu> menus =
                                 (
                                  from rm in db.Sys_Relation
                                  from m in db.Sys_Menu
                                  where rm.RelationOne==RoleId
                                  && rm.RelationTwo == m.Id
                                  && rm.RelationName == "Role-Menu"
                                  && m.IsDelete == false
                                  && rm.IsDelete == false
                                  select m
                                     ).OrderBy(o => o.SortCode).ToList();
            return menus;
        }
    }
}
