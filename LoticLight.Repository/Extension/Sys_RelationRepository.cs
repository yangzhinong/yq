using LoticLight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Repository
{
    public partial class Sys_RelationRepository
    {
        private static Model.Entities db = (Entities)RepositoryFactory.GetCurrentDbContext();
        public static void DeleteRoleAuthorize(string roleId, List<Model.Sys_Menu> deleteMenus, List<Model.Sys_Action> deleteAtions, List<Model.Sys_GridView> deleteViews)
        {

            List<Model.Sys_Relation> deleteList = new List<Model.Sys_Relation>();

            foreach (var item in deleteMenus)
            {
                var obj = Sys_RelationRepository.Instance.LoadEntitiesNoTracking(r => r.RelationOne == roleId && r.RelationTwo == item.Id && r.RelationName == "Role-Menu").FirstOrDefault();
                if (obj != null) deleteList.Add(obj);
            }
            foreach (var item in deleteAtions)
            {
                var obj = Sys_RelationRepository.Instance.LoadEntitiesNoTracking(r => r.RelationOne == roleId && r.RelationTwo == item.Id && r.RelationName == "Role-Action").FirstOrDefault();
                if (obj != null) deleteList.Add(obj);
            }
            foreach (var item in deleteViews)
            {
                var obj = Sys_RelationRepository.Instance.LoadEntitiesNoTracking(r => r.RelationOne == roleId && r.RelationTwo == item.Id && r.RelationName == "Role-View").FirstOrDefault();
                if (obj != null) deleteList.Add(obj);
            }

            Sys_RelationRepository.Instance.DeleteEntities(deleteList.ToList());
        }

       
    }
}
