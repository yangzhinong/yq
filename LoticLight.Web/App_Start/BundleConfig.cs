using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace LoticLight.Web
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
         
            ///Jquery
            bundles.Add(new ScriptBundle("~/Content/lib/jquery/JqueryJS").Include(
                "~/Content/lib/jquery/jquery-2.0.3.min.js",
                   "~/Content/lib/cookie/jquery.cookie.js"
                     ));

            ///通用
            bundles.Add(new StyleBundle("~/Content/lib/utils/CSS/utilsCSS").Include(
                        "~/Content/lib/utils/CSS/font-awesome.css",
                        "~/Content/lib/utils/CSS/learun-ui.css"
                     
                        ));
            bundles.Add(new ScriptBundle("~/Content/lib/utils/JS/utilsJS").Include(
                "~/Content/lib/utils/JS/learun-ui.js",
                "~/Content/lib/utils/JS/validator.js"
                 
                      ));
            ///首页(默认风格)
            bundles.Add(new StyleBundle("~/Content/Moulds/Home/CSS/homeCSS").Include(
                       "~/Content/Moulds/Home/CSS/index.css"
                       ));
            bundles.Add(new ScriptBundle("~/Content/Moulds/Home/JS/homeJS").Include(
                 "~/Content/Moulds/Home/JS/main.js"
                      ));
            ///首页(风尚版)
            bundles.Add(new StyleBundle("~/Content/Moulds/HomeLTE/CSS/homeLTECSS").Include(
                       "~/Content/Moulds/HomeLTE/CSS/index.css",
                       "~/Content/Moulds/HomeLTE/CSS/skins/_all-skins.css",
                       "~/Content/Moulds/HomeLTE/CSS/bootstrap.min.css",
                       "~/Content/Moulds/HomeLTE/CSS/font-awesome.min.css",
                       "~/Content/Moulds/HomeLTE/CSS/learun-ui.css"
                       ));
            bundles.Add(new ScriptBundle("~/Content/Moulds/HomeLTE/JS/homeLTEJS").Include(
                 "~/Content/Moulds/HomeLTE/index.js",
                 "~/Content/Moulds/HomeLTE/bootstrap.min.js"
                      ));
            //弹出框
            bundles.Add(new ScriptBundle("~/Content/lib/dialog/dialogJS").Include(
                "~/Content/lib/dialog/dialog.js"
                     ));
            //jquery-ui
            bundles.Add(new StyleBundle("~/Content/lib/jquery-ui/juiCSS").Include(
                      "~/Content/lib/jquery-ui/jquery-ui.min.css"
                      ));
            bundles.Add(new ScriptBundle("~/Content/lib/jquery-ui/juiJS").Include(
                 "~/Content/lib/jquery-ui/jquery-ui-1.11.4.js"
                      ));
            //bootstrap
            bundles.Add(new StyleBundle("~/Content/lib/bootstrap/bootstrapCSS").Include(
                      "~/Content/lib/bootstrap/bootstrap.min.css",
                      "~/Content/lib/bootstrap/bootstrap.extension.css"

                      ));
            bundles.Add(new ScriptBundle("~/Content/lib/bootstrap/bootstrapJS").Include(
                 "~/Content/lib/bootstrap/bootstrap.min.js"
                      ));

            //jquery.layout
            bundles.Add(new ScriptBundle("~/Content/lib/jquery-layout/jquerylayoutJS").Include(
               "~/Content/lib/jquery-layout/jquery.layout.js"
                    ));
            //datepicker
            //bundles.Add(new ScriptBundle("~/Content/lib/datepicker/datepickerJS").Include(
            // "~/Content/lib/datepicker/WdatePicker.js",
            //    "~/Content/lib/datepicker/DatePicker.js"
                  
            //      ));
            //tree
            bundles.Add(new StyleBundle("~/Content/lib/tree/treeCSS").Include(
                     "~/Content/lib/tree/tree.css"
                     ));
            bundles.Add(new ScriptBundle("~/Content/lib/tree/treeJS").Include(
                 "~/Content/lib/tree/tree.js"
                      ));
            //jqgrid
            bundles.Add(new StyleBundle("~/Content/lib/jqgrid/jqgridCSS").Include(
                     "~/Content/lib/jqgrid/jqgrid.css"
                     ));
            bundles.Add(new ScriptBundle("~/Content/lib/jqgrid/jqgridJS").Include(
                 "~/Content/lib/jqgrid/grid.locale-cn.js",
                 "~/Content/lib/jqgrid/jqgrid.js"
                      ));
            //datetime
            bundles.Add(new StyleBundle("~/Content/lib/datetime/datetimeCSS").Include(
                   "~/Content/lib/datetime/pikaday.css"
                   ));

            //wizard
            bundles.Add(new StyleBundle("~/Content/lib/wizard/wizardCSS").Include(
                     "~/Content/lib/wizard/wizard.css"
                     ));
            bundles.Add(new ScriptBundle("~/Content/lib/wizard/wizardJS").Include(
                 "~/Content/lib/wizard/wizard.js"
                      ));
            ///在线编辑器Kindeditor
            bundles.Add(new StyleBundle("~/Content/lib/kindeditor-4.1.10/kindeditorCSS").Include(
                     "~/Content/lib/kindeditor-4.1.10/themes/default/default.css",
                     "~/Content/lib/kindeditor-4.1.10/plugins/code/prettify.css"
                     ));
            bundles.Add(new ScriptBundle("~/Content/lib/kindeditor-4.1.10/kindeditorJS").Include(
                 "~/Content/lib/kindeditor-4.1.10/kindeditor.js",
                 "~/Content/lib/kindeditor-4.1.10/lang/zh_CN.js",
                 "~/Content/lib/kindeditor-4.1.10/plugins/code/prettify.js"                 
                      ));
            ///上传控件
            bundles.Add(new ScriptBundle("~/Content/lib/FileUpload/fileuploadJS").Include(
                 "~/Content/lib/FileUpload/ajaxfileupload.js"
                      ));
            //图表插件
            bundles.Add(new ScriptBundle("~/Content/lib/charts/ChartsJS").Include(
                 "~/Content/lib/charts/Chart.js"
                      ));

            bundles.Add(new ScriptBundle("~/Content/lib/jqform").Include(
                "~/Content/lib/jqform/jquery.form.min.js"
                     ));
        }

   
    
    }
}