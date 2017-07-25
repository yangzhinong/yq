using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public partial class Sys_GridViewRepository
    {

        public static List<Model.Sys_GridView> GetViewsByUserId(string Id)
        {
            Model.Entities db = (Entities)RepositoryFactory.GetCurrentDbContext();
            List<Model.Sys_GridView> views =
                       (from ur in db.Sys_Relation
                        from r in db.Sys_Role
                        from rv in db.Sys_Relation
                        from v in db.Sys_GridView
                        where ur.RelationOne == Id
                        && r.Id == ur.RelationTwo
                        && r.Id == rv.RelationOne
                        && rv.RelationTwo == v.Id
                        && ur.RelationName == "User-Role"
                        && rv.RelationName == "Role-View"
                         && v.IsDelete == false
                        && r.IsDelete == false
                        && ur.IsDelete == false
                        && rv.IsDelete == false
                        select v
                           ).ToList();
            return views;

        }




        public static List<Model.Sys_GridView> GetRoleViews(string RoleId)
        {
            Model.Entities db = (Entities)RepositoryFactory.GetCurrentDbContext();
            List<Model.Sys_GridView> actions =
                       (
                        from ra in db.Sys_Relation
                        from a in db.Sys_GridView
                        where ra.RelationOne == RoleId
                         && ra.RelationTwo == a.Id
                         && ra.RelationName == "Role-View"
                          && a.IsDelete == false
                         && ra.IsDelete == false
                        select a
                           ).ToList();
            return actions;
        }
    
    
    }
}
