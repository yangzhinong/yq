using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;





namespace LoticLight.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          //  BundleTable.EnableOptimizations = true;
            //区域注册
            AreaRegistration.RegisterAllAreas();
            //路由注册
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //过滤器[异常拦截|日志输出]
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //打包压缩资源文件,CSS、JS
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
