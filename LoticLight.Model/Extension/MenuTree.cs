using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Model
{
    public class MenuTree
    {
      public  string id { set; get; }
       public  string text { set; get; }
       public  string value { set; get; }
      public   string img { set; get; }
      public   string parentnodes { set; get; }
      public bool showcheck { set; get; }
      public bool isexpand { set; get; }
      public bool complete { set; get; }
       public  bool hasChildren { set; get; }
       public int checkstate { set; get; }
       public List<object> ChildNodes { set; get; }

    }

}
