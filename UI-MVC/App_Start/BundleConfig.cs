using System.Web;
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
                  "~/Scripts/bootstrap.min.js"));

      bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                  "~/Content/bootstrap.css"));

      bundles.Add(new ScriptBundle("~/bundles/adminator").Include("~/Scripts/vendor.js", "~/Scripts/bundle.js")); 
    }
  }
}
