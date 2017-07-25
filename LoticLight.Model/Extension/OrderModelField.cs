using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Model
{

    /// <summary>
    /// 动态排序
    /// </summary>
   public class OrderModelField
    {
        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string propertyName { get; set; }
        /// <summary>
        /// true为降序false为升序
        /// </summary>
        public bool IsDESC { get; set; }
    }
}
