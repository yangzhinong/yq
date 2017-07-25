using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoticLight.Utility
{
    /// <summary>
    /// 构造树形表格Json
    /// </summary>
    public static  class TreeGridJson
    {
        public static int lft = 1, rgt = 1000000;
        /// <summary>
        /// 转换树形Json
        /// </summary>
        /// <param name="ListData"></param>
        /// <param name="index"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public static string TreeJson(List<TreeGridEntity> ListData, int index, string ParentId)
        {
            StringBuilder sb = new StringBuilder();
            var ChildNodeList = ListData.FindAll(t => t.parentId == ParentId);
            if (ChildNodeList.Count > 0) { index++; }
            foreach (TreeGridEntity entity in ChildNodeList)
            {
                string strJson = entity.entityJson;
                strJson = strJson.Insert(1, "\"level\":" + index + ",");
                strJson = strJson.Insert(1, "\"isLeaf\":" + (entity.hasChildren == true ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"expanded\":" + (entity.expanded).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"lft\":" + lft++ + ",");
                strJson = strJson.Insert(1, "\"rgt\":" + rgt-- + ",");
                sb.Append(strJson);
                sb.Append(TreeJson(ListData, index, entity.id));
            }

            return sb.ToString().Replace("}{", "},{");
        }

        public static string TreeJson(this List<TreeGridEntity> listData)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"rows\":[");
            sb.Append(TreeJson(listData, -1, "0"));
            sb.Append("]}");
            return sb.ToString();
        }
    }
}
