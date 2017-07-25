using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Model
{
    public class Region
    {
        public string id{get;set;}

        public string ParentId { get; set; }

        public string RegionName { get; set; }

        public string value { get; set; }
        public string CenterLatitude { get; set; }

        public string CenterLongitude { get; set; }

        public string Radius { get; set; }
        
        public bool isexpanded { get; set; }

        public bool complete { get; set; }

        public bool hasChildren { get; set; }

        public List<object> ChildNodes { get; set; }
    }
}
