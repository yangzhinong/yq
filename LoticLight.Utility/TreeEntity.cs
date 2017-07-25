using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Utility
{
    public class TreeEntity
    {
        public string parentId { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public int? checkstate { get; set; }
        public bool showcheck { get; set; }
        public bool complete { get; set; }
        //是否展开
        public bool isexpand { get; set; }
        //是否有子节点
        public bool hasChildren { get; set; }
        //级
        public int? Level { get; set; }
        /// <summary>
        /// 自定义属性
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// 自定义属性值
        /// </summary>
        public string AttributeValue { get; set; }
        /// <summary>
        /// 自定义属性A
        /// </summary>
        public string AttributeA { get; set; }
        /// <summary>
        /// 自定义属性值A
        /// </summary>
        public string AttributeValueA { get; set; }
    }
}
