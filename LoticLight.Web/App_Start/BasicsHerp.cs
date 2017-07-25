using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoticLight.Web.App_Start
{
    public class BasicsHerp
    {

        public static Model.Entities db = new Model.Entities();

        /// <summary>
        /// 遍历区域的子节点
        /// </summary>
        /// <param name="p_id"></param>
        /// <returns></returns>
        //public static List<Model.Sys_Region> GetSonID(int p_id, List<Model.Sys_Region> alllist)
        //{

        //    var query = from c in alllist
        //                where c.ParentID == p_id
        //                select c;

        //    List<Model.Sys_Region> newlist = new List<Model.Sys_Region>();
        //    foreach (Model.Sys_Region item in query.ToList())
        //    {
        //        newlist.Add(item);
        //        newlist = newlist.Concat(GetSonID(item.ID, alllist)).ToList();
        //    }
        //    return newlist;
        //}
        ///// <summary>
        ///// 遍历区域的父节点
        ///// </summary>
        ///// <param name="p_id"></param>
        ///// <returns></returns>
        //public static List<Model.Sys_Region> GetSthID(int prent_id, List<Model.Sys_Region> alllist)
        //{

        //    var query = from c in alllist
        //                where c.ID == prent_id
        //                select c;

        //    List<Model.Sys_Region> newlist = new List<Model.Sys_Region>();
        //    foreach (Model.Sys_Region item in query.ToList())
        //    {
        //        newlist.Add(item);
        //        newlist = newlist.Concat(GetSthID(item.ParentID, alllist)).ToList();
        //    }
        //    return newlist;
        //}
        ///// <summary>
        ///// 获取当前用户所有的区域权限的id
        ///// </summary>
        ///// <returns></returns>
        //public static List<int> GetRegion()
        //{
        //    var rlist = db.Region_User_Role.Where(u => u.U_ID == WebSession.CurrentAccount.User.ID).ToList();
        //    int rid = 0;
        //    var alllist = db.Sys_Region.ToList();
        //    List<int> child = new List<int>();
        //    if (rlist.Count > 0)
        //    {
        //        foreach (var i in rlist)
        //        {
        //            child.Add((int)i.R_ID);
        //            rid = (int)i.R_ID;
        //            var listdata = GetSonID(rid, alllist);
        //            if (listdata.Count == 0)
        //            {
        //                child.Add((int)i.R_ID);
        //            }
        //            foreach (var item in listdata)
        //            {
        //                child.Add(item.ID);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var listdata = GetSonID(rid, alllist);
        //        foreach (var item in listdata)
        //        {
        //            child.Add(item.ID);
        //        }
        //    }
        //    return child;
        //}
        ///// <summary>
        ///// 获取当前用户的区域权限模块
        ///// </summary>
        ///// <returns></returns>
        //public static List<Model.Sys_Region> GetRegionRole()
        //{

        //    List<Model.Sys_Region> newlsit = new List<Model.Sys_Region>();
        //    var alllist = db.Sys_Region.ToList(); ;
        //    var rolechild = GetRegion();//获取用户权限区域ID序列
        //    for (int i = 0; i < rolechild.Count; i++)
        //    {
        //        var sonlist = GetSonID(rolechild[i], alllist);//获取当前节点的子节点
        //        var sthlist = GetSthID(rolechild[i], alllist);//获取当前节点的父节点               

        //        foreach (var item in sthlist)
        //        {
        //            if (newlsit.Count(o => o.ID == item.ID) == 0)
        //            { newlsit.Add(item); }
        //        }
        //        foreach (var item in sonlist)
        //        {
        //            if (newlsit.Count(o => o.ID == item.ID) == 0)
        //            { newlsit.Add(item); }
        //        }
        //    }
        //    List<Model.Sys_Region> lsitnew = new List<Model.Sys_Region>();
        //    var list = GetSonID(newlsit.Find(o => o.ParentID == 0).ID, newlsit);
        //    var data = newlsit.FirstOrDefault(u => u.ParentID == 0);
        //    lsitnew.Add(data);
        //    foreach (var item in list)
        //    {
        //        lsitnew.Add(item);
        //    }
        //    return lsitnew;
        //}

    }
}