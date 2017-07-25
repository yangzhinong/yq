using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public partial class Sys_ActionRepository
    {

        public static List<Model.Sys_Action> GetActionsByUserId(string Id)
        {
            Model.Entities db = (Entities)RepositoryFactory.GetCurrentDbContext();
            List<Model.Sys_Action> actions =
                       (from ur in db.Sys_Relation
                        from r in db.Sys_Role
                        from ra in db.Sys_Relation
                        from a in db.Sys_Action
                        where ur.RelationOne == Id
                        && r.Id == ur.RelationTwo
                        && r.Id == ra.RelationOne
                        && ra.RelationTwo == a.Id
                        && ur.RelationName == "User-Role"
                        && ra.RelationName == "Role-Action"
                        && a.IsDelete == false
                        && r.IsDelete == false
                        && ur.IsDelete == false
                        && ra.IsDelete == false
                        select a
                           ).ToList();
            return actions;

        }

        public static List<Model.Sys_Action> GetRoleActions(string RoleId)
        {
            Model.Entities db = (Entities)RepositoryFactory.GetCurrentDbContext();
            List<Model.Sys_Action> actions =
                       (
                        from ra in db.Sys_Relation
                        from a in db.Sys_Action
                        where ra.RelationOne == RoleId
                         && ra.RelationTwo == a.Id
                         && ra.RelationName == "Role-Action"
                          && a.IsDelete == false
                         && ra.IsDelete == false
                        select a
                           ).ToList();
            return actions;
        }
    
    
    }
}
