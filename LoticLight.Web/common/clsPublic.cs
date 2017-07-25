using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

namespace LoticLight.Web
{
    public class clsPublic
    {

        public static FileStreamResult ExportExcel(DataTable dt, string toFile, string Title)
        {
            if (toFile.Length > 4)
            {
                if (toFile.Substring(toFile.Length-4,4) != ".xls")
                {
                    toFile = toFile + ".xls";
                }
            }

            using (var ms = ExcelTool.DataTableToExcel.RenderDataTableToExcel(dt, Title))
            {
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                
                var fs= new FileStreamResult(ms, "application/vnd.ms-excel");
                fs.FileDownloadName = toFile;
                return fs;
            }
        }

        public static void DownLoadFile(string sfile, string oldFile = "")
        {
            var fs = new System.IO.FileStream(sfile, System.IO.FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];

            if (string.IsNullOrEmpty(oldFile))
            {
                oldFile = System.IO.Path.GetFileName(sfile);
            }

            var res = HttpContext.Current.Response;
            res.ContentType = "application/octet-stream";
            res.AddHeader("Content-Disposition", "attachment; filename=" +
                       HttpUtility.UrlEncode(oldFile, System.Text.Encoding.UTF8));
            res.BinaryWrite(bytes);
            res.Flush();
            res.End();
        }

        
    }
}