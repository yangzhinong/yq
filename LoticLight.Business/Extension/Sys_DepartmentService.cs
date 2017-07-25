using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoticLight.Model;
using LoticLight.Utility;
using System.Threading.Tasks;

namespace LoticLight.Business 
{
    public partial class Sys_DepartmentService
    {
      
        public static List<TreeGridEntity> GetTreeList(string keyword)
        {
            var data = Business.Sys_DepartmentService.Instance.LoadEntities(t=>t.DeptName.Contains(keyword)|| t.code.Contains(keyword)).ToList();
            var treeList = new List<TreeGridEntity>();
            foreach (Model.Sys_Department item in data)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.id = item.Id;
                tree.hasChildren = hasChildren;
                if (keyword == "")
                    tree.parentId = item.ParentId;
                else
                    tree.parentId = "0";
                tree.expanded = true;
                tree.entityJson = item.ToJson();
                treeList.Add(tree);
            }
            return treeList;
        }

        /// <summary>
        /// 得到上级产品类型
        /// </summary>
        /// <param name="Id">开始节点id，0为开始id</param>
        /// <param name="list">菜单列表</param>
        /// <returns>List:MenuTree</returns>
        public static List<object> GetTree(string Id, List<Model.Sys_Department> list)
        {

            var newlist = new List<object>();
            if(Id=="0")
            { 
            Model.MenuTree temp = new MenuTree();
            temp.id = "0";
            temp.value = "0";
            temp.text = "顶层节点";
            temp.parentnodes = "0";
            temp.showcheck = true;
            temp.isexpand = true;
            temp.complete = true;
            temp.checkstate = 0;
            temp.hasChildren = false;
            newlist.Add(temp);
            }
            var clist = list.FindAll(o => o.ParentId == Id);
            foreach (var item in clist)
            {
                Model.MenuTree newitem = new MenuTree()
                {
                    id = item.Id,
                    text = item.DeptName,
                    value = item.Id,
                    parentnodes = item.ParentId,
                    showcheck = true,
                    isexpand = true,
                    complete = true,
                    checkstate = 0,
                    hasChildren = list.Count(o => o.ParentId == item.Id) > 0 ? true : false
                };
                newitem.ChildNodes = GetTree(newitem.id, list);
                newlist.Add(newitem);
            }
            return newlist;
        }
    }
}
