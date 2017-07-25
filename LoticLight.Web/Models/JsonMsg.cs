using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web
{
    public class JsonMsg
    {
        public int code { get; set; }

        public string msg { get; set; }

        public string data { get; set; }

        public static JsonResult OkMsg(string msg = "操作成功", string data = "")
        {
            var o = new JsonMsg();
            o.code = 1;
            o.msg = msg;
            o.data = data;

            var res = new JsonResult();
            res.Data = o;

            return res;
        }

        public static JsonResult InfoMsg(int code, string msg = "", string data = "")
        {
            var o = new JsonMsg();
            o.code = code;
            o.msg = msg;
            o.data = data;

            var res = new JsonResult();
            res.Data = o;

            return res;
        }

        public static JsonResult ErrMsg(string msg, string data = "")
        {
            var o = new JsonMsg();
            o.code = 0;
            o.msg = msg;
            o.data = data;

            var res = new JsonResult();
            res.Data = o;
            return res;
        }
    }
}