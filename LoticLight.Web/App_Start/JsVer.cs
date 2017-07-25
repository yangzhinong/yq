﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace LoticLight.Web
{
    public static class TimestampedContentExtensions
    {
        private static string dVer = DateTime.Now.ToString("yyyyMMddHHmmss");
        public static string VersionedContent(this UrlHelper helper, string contentPath)
        {
            var context = helper.RequestContext.HttpContext;

            if (context.Cache[contentPath] == null)
            {
                var physicalPath = context.Server.MapPath(contentPath);
                var version = @"v=" + new FileInfo(physicalPath).LastWriteTime.ToString(@"yyyyMMddHHmmss");

                var translatedContentPath = helper.Content(contentPath);

                var versionedContentPath =
                    contentPath.Contains(@"?")
                        ? translatedContentPath + @"&" + version
                        : translatedContentPath + @"?" + version;

                context.Cache.Add(physicalPath, version, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero,
                    CacheItemPriority.Normal, null);

                context.Cache[contentPath] = versionedContentPath;
                return versionedContentPath;
            }
            else
            {
                return context.Cache[contentPath] as string;
            }
        }

        public static string JsVer(this UrlHelper helper, string contentPath)
        {
            var context = helper.RequestContext.HttpContext;
            var physicalPath = context.Server.MapPath(contentPath);
            var version = @"v=" + new FileInfo(physicalPath).LastWriteTime.ToString(@"yyyyMMddHHmmss");
            return version;

        }
        public static string AppVer(this UrlHelper helper)
        {

            return dVer;

        }
    }
}