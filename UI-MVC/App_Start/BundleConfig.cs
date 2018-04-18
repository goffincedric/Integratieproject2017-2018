﻿using System.Web;
using System.Web.Optimization;

namespace UI_MVC
{
  public class BundleConfig
  {
    // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.IgnoreList.Clear();

      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/Scripts/jquery-3.3.1.js"));

      bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                  "~/Scripts/bootstrap.js"));

      bundles.Add(new ScriptBundle("~/bundles/adminator").Include("~/Scripts/load.js","~/Scripts/vendor.js", "~/Scripts/bundle.js"));

      bundles.Add(new ScriptBundle("~/bundles/changetheme").Include("~/Scripts/Changetheme.js"));

      bundles.Add(new ScriptBundle("~/bundles/contact").Include("~/Scripts/contact.js"));

      bundles.Add(new ScriptBundle("~/bundles/hidecontent").Include("~/Scripts/hidecontent.js"));

      bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include("~/Content/bootstrap.css"));
    }
  }
}
