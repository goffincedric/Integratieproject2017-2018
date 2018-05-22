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


            bundles.Add(new ScriptBundle("~/bundles/adminator").Include("~/Scripts/load.js", "~/Scripts/vendor.js",
                "~/Scripts/bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/changetheme").Include("~/Scripts/Changetheme.js"));

            bundles.Add(new ScriptBundle("~/bundles/contact").Include("~/Scripts/contact.js"));

            bundles.Add(new ScriptBundle("~/bundles/AccountControl").Include("~/Scripts/AccountControl.js"));

            bundles.Add(new ScriptBundle("~/bundles/Dashboard").Include("~/Scripts/Dashboard/DashboardControl.js",
                "~/Scripts/Dashboard/Dashboard.js"));

            bundles.Add(new StyleBundle("~/Content/dashboardcss").Include("~/Content/Styling/Grid/gridstack.css",
                "~/Content/Styling/Grid/gridstack-extra.css",
                "~/Content/Styling/Wizard/material-bootstrap-wizard.css"));
        }
    }
}