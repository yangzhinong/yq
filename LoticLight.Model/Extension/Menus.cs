using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Model
{
    /// <summary>
    /// 前端使用的菜单类
    /// </summary>
   public class Menus
    {
        /// <summary>
        /// Id
        /// </summary>
       public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string UrlAddress { get; set; }
        public bool IsLeaf { get; set; }
        public string SortCode { get; set; }
     
    }
}
