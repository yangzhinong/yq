using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoticLight.Business.Basics
{
    public class clsWeek
    {

        public static DateTime GetMonday() {
            var d = DateTime.Now.Date;
            var iDay = (int)d.DayOfWeek;
            return d.AddDays(1 - iDay);
        }
        public static DateTime GetFirday()
        {
            return GetMonday().AddDays(4);
        }

        public static string FormatDate(DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        public static int WeekInt()
        {
          var i=  SqlHelper.ExecuteScalar(SqlHelper.GetConnSting(), System.Data.CommandType.Text, "SELECT DATEDIFF(WEEK,0,GETDATE())");

            return Convert.ToInt32(i);
        }

        public static string WeekName(int weekInt)
        {
            var ret = "";
            try
            {
                ret = Convert.ToString(SqlHelper.ExecuteScalar(SqlHelper.GetConnSting(), System.Data.CommandType.StoredProcedure, "dbo.pGetWeekName", new System.Data.SqlClient.SqlParameter("@@weekInt", weekInt)));

            } catch { }
            return ret;
        }

        public static string LastWeekName()
        {
            var ret = "";
            try
            {
                ret = Convert.ToString(SqlHelper.ExecuteScalar(SqlHelper.GetConnSting(), System.Data.CommandType.StoredProcedure, "dbo.pGetLastWeekName"));

            }
            catch { }
            return ret;
        }



        public static DateTime? GetDateTimeFromString(string sD)
        {
            DateTime? d = null;
            try
            {
                if (!string.IsNullOrEmpty(sD))
                {
                    d = DateTime.Parse(sD);
                }
            }
            catch { }

            return d;
        }
    }
}
