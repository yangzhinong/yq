using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoticLight.Model
{
    /// <summary>
    /// 系统初始化模型
    /// </summary>
    public class initData
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<Model.Menus> menus { get; set; }
        /// <summary>
        /// 操作列表
        /// </summary>
        public Dictionary<string, object> menusActions { get; set; }

        /// <summary>
        /// 数据表视图
        /// </summary>
        public Dictionary<string, object> menusViews { get; set; }

    }



}