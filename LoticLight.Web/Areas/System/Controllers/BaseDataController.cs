using LoticLight.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web.Areas.System.Controllers
{
    /// <summary>
    /// 本控制器同通用的数据：下拉菜单数据、列表树数据等
    /// </summary>
    public class BaseDataController :BaseController
    {
        /// <summary>
        /// 获取全部菜单
        /// </summary>
        /// <returns>树状数据</returns>
        public ActionResult TreeData(string keyword)
        {
            var data = Business.Sys_MenuService.GetmenuTree(keyword);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 图标列表
        /// </summary>
        /// <returns></returns>
        public ActionResult IconList()
        {
            return View();
        }

        /// <summary>
        /// 日志列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LogIndex()
        {
            return View();
        }

        public ActionResult LogData(Pagination pagination, string keyword)
        {

            //默认排序
            if (string.IsNullOrEmpty(pagination.sidx))
            {
                pagination.sidx = "CreatTime";
                pagination.sord = "desc";
 
            }
            var data = Business.Sys_LogService.Instance.LoadPagerEntities(
                          o => (o.OperateType.Contains(keyword) || string.IsNullOrEmpty(keyword))
                          && (o.MenuName.Contains(keyword) || string.IsNullOrEmpty(keyword)),
                          pagination.page, pagination.rows, out pagination.records,
                          new OrderModelField[]{ 
                            
                              new OrderModelField() { propertyName = pagination.sidx, IsDESC = pagination.sord == "desc" ? true : false },
                          }
                         
                          );
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,

            };
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取下拉框数据字典
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ActionResult GetDataItemListJson(string Type)
        {
            var data = Business.Sms_DataDictionaryService.Instance.LoadEntitiesNoTracking(o => (o.Type.Trim() == Type));

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 上传文件，并且返回文件URL路径
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        public ActionResult UploadFile(string dir, HttpPostedFileBase imgFile)
        {
            //文件保存目录路径
            String savePath = Server.MapPath("~/ContentFile/");

            //文件保存目录URL
            String saveUrl = "/ContentFile/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb,mp4,ogg");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1024000000;

            if (imgFile == null)
            {
                return Content("请选择文件。");
            }


            if (!Directory.Exists(savePath))
            {
                return Content("上传目录不存在。");
            }

            String dirName = dir;
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return Content("目录名不正确。");
            }
            savePath += dirName + "/";
            saveUrl += dirName + "/";
            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Content("上传文件大小超过限制。");
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Content("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            //创建文件夹

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            savePath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = savePath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            var result = new
            {
                error = 0,
                url = fileUrl

            };
            return Json(result, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// 文件管理方法，返回所有上传文件路径
        /// </summary>
        /// <returns></returns>
        public ActionResult FilesManger(string dir, string path, string order)
        {
            //根目录路径
            String rootPath = Server.MapPath("~/ContentFile/");

            //根目录URL，
            String rootUrl = "/ContentFile/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            String dirPath = rootPath;
            String dirName = dir;
            if (!String.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    return Content("无效的目录名称");
                }
                dirPath += dirName + "/";
                rootUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            //根据path参数，设置各路径和URL
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return Content("不允许访问");

            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return Content("无效参数");
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                return Content("目录不存在");
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dirt = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dirt.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dirt.Name;
                hash["datetime"] = dirt.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 名字排序
        /// </summary>
        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        /// <summary>
        /// 大小排序
        /// </summary>
        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        /// <summary>
        /// 类型排序
        /// </summary>
        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
    }
}