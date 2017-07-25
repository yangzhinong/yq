using LoticLight.Model;
using LoticLight.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoticLight.Utility;


namespace LoticLight.Business
{
    public partial class Sys_MenuService
    {

        #region 公共方法
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        public static List<Model.Menus> GetmenusByUserId(string Id)
        {
            return Repository.Sys_MenuRepository.GetmenusByUserId(Id);
        }
        /// <summary>
        /// 获取角色菜单
        /// </summary>
        /// <param name="Id">角色Id</param>
        /// <returns></returns>
        public static List<Model.Sys_Menu> GetmenusByRoleId(string Id)
        {
            return Repository.Sys_MenuRepository.GetmenusByRoleId(Id);
        }

        /// <summary>
        /// 得到角色菜单树
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<object> GetRoleMenusTree(string Id)
        {
            List<object> _MenuTree;
            _MenuTree = GetChildNode("0", Sys_MenuService.Instance.LoadEntitiesNoTracking().ToList(), GetmenusByRoleId(Id));
            return _MenuTree;
        }

        /// <summary>
        /// 得到菜单树
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<object> GetmenuTree(string keyword = "")
        {


            //todo:关键字搜索未实现
            List<object> _MenuTree;
            _MenuTree = GetChildNode("0", Sys_MenuService.Instance.LoadEntitiesNoTracking().ToList());

            _MenuTree.Insert(0, new MenuTree()
                {
                    id = "0",
                    text = "系统模块",
                    value = "-1",
                    img = "fa " + "fa-desktop",//todo:图标替换
                    parentnodes = "-1",
                    showcheck = true,
                    isexpand = true,
                    complete = true,
                    hasChildren = false
                });

            return _MenuTree;
        }


        /// <summary>
        /// 获取角色菜单操作列表树
        /// </summary>
        /// <param name="RoleId">角色Id</param>
        /// <returns></returns>
        public static List<object> GetRoleMenusActionsTree(string RoleId)
        {

            var roleActions = Business.Sys_ActionService.GetRoleActions(RoleId);
            var allActions = Business.Sys_ActionService.Instance.LoadEntities().ToList();
            var menus = Sys_MenuService.Instance.LoadEntities().ToList();
            return GetChildNode("0", menus, roleActions, allActions);
        }

        /// <summary>
        /// 获取角色菜单视图列表树
        /// </summary>
        /// <param name="RoleId">角色Id</param>
        /// <returns></returns>
        public static List<object> GetRoleMenusViewsTree(string RoleId)
        {

            var roleViews = Business.Sys_GridViewService.GetRoleViews(RoleId);
            var allViews = Business.Sys_GridViewService.Instance.LoadEntitiesNoTracking().ToList();
            var menus = Sys_MenuService.Instance.LoadEntitiesNoTracking().ToList();
            var data = GetChildNode("0", menus, roleViews, allViews);
            return data;
        }
        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleIds"></param>
        /// <param name="moduleButtonIds"></param>
        /// <param name="moduleColumnIds"></param>
        public static void SaveAuthorize(string roleId, string moduleIds, string moduleButtonIds, string moduleColumnIds)
        {
            ///保存权限
            string[] arrayModuleId = moduleIds.Split(',');
            string[] arrayModuleButtonId = moduleButtonIds.Split(',');
            string[] arrayModuleColumnId = moduleColumnIds.Split(',');
            ///当前权限
            var roleMenus = Business.Sys_MenuService.GetmenusByRoleId(roleId);
            var roleAtions = Business.Sys_ActionService.GetRoleActions(roleId);
            var roleViews = Business.Sys_GridViewService.GetRoleViews(roleId);

            //移除的权限
            var deleteMenus = from o in roleMenus
                              where arrayModuleId.Contains(o.Id) == false
                              select o;
            var deleteAtions = from o in roleAtions
                               where arrayModuleButtonId.Contains(o.Id) == false
                               select o;
            var deleteViews = from o in roleViews
                              where arrayModuleColumnId.Contains(o.Id) == false
                              select o;
            Repository.Sys_RelationRepository.DeleteRoleAuthorize(roleId, deleteMenus.ToList(), deleteAtions.ToList(), deleteViews.ToList());

            //添加的权限

            List<Model.Sys_Relation> addlist = new List<Sys_Relation>();

            foreach (var item in arrayModuleId)
            {
                if (roleMenus.Count(o => o.Id == item) == 0)
                {
                    var newobj = new Model.Sys_Relation()
                    {
                        RelationName = "Role-Menu",
                        RelationOne = roleId.Trim(),
                        RelationTwo = item
                    };
                    addlist.Add(newobj);
                }

            }
            foreach (var item in arrayModuleButtonId)
            {
                if (roleAtions.Count(o => o.Id == item) == 0)
                {
                    var newobj = new Model.Sys_Relation()
                    {
                        RelationName = "Role-Action",
                        RelationOne = roleId.Trim(),
                        RelationTwo = item
                    };
                    addlist.Add(newobj);
                }

            }
            foreach (var item in arrayModuleColumnId)
            {
                if (roleViews.Count(o => o.Id == item) == 0)
                {
                    var newobj = new Model.Sys_Relation()
                    {
                        RelationName = "Role-View",
                        RelationOne = roleId.Trim(),
                        RelationTwo = item
                    };
                    addlist.Add(newobj);
                }

            }

            Repository.Sys_RelationRepository.Instance.AddEntities(addlist);
            Instance._dbSession.SaveChanges();
        }


        /// <summary>
        /// 删除菜单以及所有下级节点
        /// </summary>
        /// <param name="MenuId"></param>
        public static void DeleteMenusAndChildren(string MenuId)
        {
            var DeleteMenus=GetChildNodeToSys_Menu(MenuId,Instance.LoadEntitiesNoTracking(o=>o.Id!="").ToList());
            var obj=Instance.FindEntities(MenuId);
            DeleteMenus.Add(obj);
            Instance.DeleteEntities(DeleteMenus);  
        }



        /// <summary>
        /// 保存菜单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Menu"></param>
        /// <param name="MenuActionsJson"></param>
        /// <param name="MenuViewsJson"></param>
        public static void SaveMenus(string keyValue, Model.Sys_Menu Menu, string MenuActionsJson, string MenuViewsJson)
        {
            var actions = MenuActionsJson.ToList<Model.Sys_Action>();
            var views = MenuViewsJson.ToList<Model.Sys_GridView>();
            if (string.IsNullOrEmpty(keyValue))
            {
                Instance.AddEntities(Menu);
            }
            else
            {
                Instance.UpdateEntities(Menu);
            }


            var actionslist = Business.Sys_ActionService.Instance.LoadEntitiesNoTracking(o => o.MenuId == Menu.Id);
            var viewslist = Business.Sys_GridViewService.Instance.LoadEntitiesNoTracking(o => o.MenuId == Menu.Id);
            //清空已删除的数据
            var actionsdelete = from a in actionslist
                                where actions.Count(o => o.Id == a.Id) == 0
                                select a;
            Sys_ActionRepository.Instance.DeleteEntities(actionsdelete.ToList());

            var viewsdelete = from a in viewslist
                              where views.Count(o => o.Id == a.Id) == 0
                              select a;
            Sys_GridViewRepository.Instance.DeleteEntities(viewsdelete.ToList());

            foreach (var item in actions)
            {
                item.ActionIcon = "fa-tag";
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.MenuId = Menu.Id;
                    item.Status = "1";
                    Sys_ActionRepository.Instance.AddEntities(item);
                }
                else
                {
                    item.MenuId = Menu.Id;
                    item.Creator = actionslist.First(o => o.Id == item.Id).Creator;
                    item.CreatTime = actionslist.First(o => o.Id == item.Id).CreatTime;
                    Sys_ActionRepository.Instance.UpdateEntities(item);
                }

            }
            foreach (var item in views)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.MenuId = Menu.Id;
                    item.Status = "1";
                    
                    Sys_GridViewRepository.Instance.AddEntities(item);
                }
                else
                {
                    item.MenuId = Menu.Id;
                    item.Creator = viewslist.First(o => o.Id == item.Id).Creator;
                    item.CreatTime = viewslist.First(o => o.Id == item.Id).CreatTime;
                    Sys_GridViewRepository.Instance.UpdateEntities(item);
                }

            }
            Instance._dbSession.SaveChanges();

        }


        #endregion
        #region 内部私有方法


        /// <summary>
        /// 得到所有子节点
        /// </summary>
        /// <param name="Id">开始节点id，0为开始id</param>
        /// <param name="list">菜单列表</param>
        /// <returns>List:MenuTree</returns>
        private static List<object> GetChildNode(string Id, List<Model.Sys_Menu> list)
        {
            var newlist = new List<object>();
            var clist = list.FindAll(o => o.ParentId == Id);
            foreach (var item in clist)
            {
                Model.MenuTree newitem = new MenuTree()
                {
                    id = item.Id,
                    text = item.MenuName,
                    value = item.Id,
                    img = "fa " + item.MenuIcon,
                    parentnodes = item.ParentId,
                    showcheck = true,
                    isexpand = item.IsExpand,
                    complete = true,
                    checkstate = 0,
                    hasChildren = list.Count(o => o.ParentId == item.Id) > 0 ? true : false
                };
                newitem.ChildNodes = GetChildNode(newitem.id, list);
                newlist.Add(newitem);
            }
            return newlist;
        }
       
       /// <summary>
       /// 得到所有子节点
       /// </summary>
       /// <param name="Id">开始节点id，0为开始id</param>
       /// <param name="list">菜单列表</param>
       /// <returns>List:Sys_Menu</returns>
         private static List<Sys_Menu> GetChildNodeToSys_Menu(string Id, List<Model.Sys_Menu> list)
        {
            var newlist = new List<Sys_Menu>();
            var clist = list.FindAll(o => o.ParentId == Id);
            var ChildNodes=new List<Sys_Menu>();
            foreach (var item in clist)
            {
                ChildNodes = GetChildNodeToSys_Menu(item.Id, list);
                newlist.Concat(ChildNodes);
            }
            return newlist;
        }
        
        /// <summary>
        /// 得到所有子节点，并根据角色权限给checkstate赋值
        /// </summary>
        /// <param name="Id">开始节点id，0为开始id</param>
        /// <param name="list">菜单列表</param>
        /// <param name="roleMenus">角色菜单列表</param>
        /// <returns></returns>
        private static List<object> GetChildNode(string Id, List<Model.Sys_Menu> list, List<Model.Sys_Menu> roleMenus)
        {
            var newlist = new List<object>();
            var clist = list.FindAll(o => o.ParentId == Id);
            foreach (var item in clist)
            {
                Model.MenuTree newitem = new MenuTree()
                {
                    id = item.Id,
                    text = item.MenuName,
                    value = item.Id,
                    img = "fa " + item.MenuIcon,
                    parentnodes = item.ParentId,
                    showcheck = true,
                    isexpand = item.IsExpand,
                    complete = true,
                    checkstate = roleMenus.Count(o => o.Id == item.Id) > 0 ? 1 : 0,
                    hasChildren = list.Count(o => o.ParentId == item.Id) > 0 ? true : false
                };
                newitem.ChildNodes = GetChildNode(newitem.id, list, roleMenus);
                newlist.Add(newitem);
            }
            return newlist;
        }

        private static List<object> GetChildNode(string Id, List<Model.Sys_Menu> list, List<Model.Sys_Action> roleAction, List<Model.Sys_Action> allAction)
        {
            var newlist = new List<object>();
            var clist = list.FindAll(o => o.ParentId == Id);
            foreach (var item in clist)
            {
                bool isleaf = list.Count(o => o.ParentId == item.Id) == 0;
                Model.MenuTree newitem = new MenuTree()
                {
                    id = item.Id,
                    text = item.MenuName,
                    value = item.Id,
                    img = "fa " + item.MenuIcon,
                    parentnodes = item.ParentId,
                    showcheck = true,
                    isexpand = item.IsExpand,
                    complete = true,
                    checkstate = 0,
                    hasChildren = list.Count(o => o.ParentId == item.Id) > 0 ? true : false
                };
                if (newitem.hasChildren)
                {
                    newitem.ChildNodes = GetChildNode(newitem.id, list, roleAction, allAction);
                    if (newitem.ChildNodes.Count(o => ((Model.MenuTree)o).checkstate == 1) > 0) newitem.checkstate = 1;
                }
                else
                {
                    var listActions = (from a in allAction.FindAll(o => o.MenuId == newitem.id).ToList()
                                       select new MenuTree()
                                       {
                                           id = a.Id,
                                           text = a.ActionName,
                                           value = a.Id,
                                           img = "fa " + a.ActionIcon,
                                           parentnodes = newitem.id,
                                           showcheck = true,
                                           isexpand = true,
                                           complete = true,
                                           checkstate = roleAction.Count(o => o.Id == a.Id) > 0 ? 1 : 0,
                                       }).ToList();
                    newitem.ChildNodes = listActions.ConvertAll(o => (object)o);

                    if (listActions.Count(o => o.checkstate == 1) > 0) newitem.checkstate = 1;
                    if (listActions.Count() > 0) newitem.hasChildren = true;
                }
                newlist.Add(newitem);
            }
            return newlist;
        }


        private static List<object> GetChildNode(string Id, List<Model.Sys_Menu> list, List<Model.Sys_GridView> roleView, List<Model.Sys_GridView> allView)
        {
            var newlist = new List<object>();
            var clist = list.FindAll(o => o.ParentId == Id);
            foreach (var item in clist)
            {
                bool isleaf = list.Count(o => o.ParentId == item.Id) == 0;
                Model.MenuTree newitem = new MenuTree()
                {
                    id = item.Id,
                    text = item.MenuName,
                    value = item.Id,
                    img = "fa " + item.MenuIcon,
                    parentnodes = item.ParentId,
                    showcheck = true,
                    isexpand = item.IsExpand,
                    complete = true,
                    checkstate = 0,
                    hasChildren = list.Count(o => o.ParentId == item.Id) > 0 ? true : false
                };
                if (newitem.hasChildren)
                {
                    newitem.ChildNodes = GetChildNode(newitem.id, list, roleView, allView);
                    if (newitem.ChildNodes.Count(o => ((Model.MenuTree)o).checkstate == 1) > 0) newitem.checkstate = 1;
                }
                else
                {
                    var listViews = (from a in allView.FindAll(o => o.MenuId == newitem.id).ToList()
                                       select new MenuTree()
                                       {
                                           id = a.Id,
                                           text = a.ViewName,
                                           value = a.Id,
                                           img = "fa " + "fa-reorder",
                                           parentnodes = newitem.id,
                                           showcheck = true,
                                           isexpand = true,
                                           complete = true,
                                           checkstate = roleView.Count(o => o.Id == a.Id) > 0 ? 1 : 0,
                                       }).ToList();
                    newitem.ChildNodes = listViews.ConvertAll(o => (object)o);

                    if (listViews.Count(o => o.checkstate == 1) > 0) newitem.checkstate = 1;
                    if (listViews.Count() > 0) newitem.hasChildren = true;
                }
                newlist.Add(newitem);
            }
            return newlist;
        }


        #endregion

    }
}
