using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business.Data
{
   public  class clsPN
    {
        public static bool ExistsPN(string pn)
        {
           var oPN=  Business.YqPNService.Instance.LoadEntities(x => x.PN == pn).FirstOrDefault();

            if (oPN !=null && oPN.PN == pn)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
