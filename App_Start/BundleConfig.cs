using System.Web;
using System.Web.Optimization;

namespace Online_Laundry_Management_System
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/chart.js",
                        "~/Scripts/codemirror.js",
                        "~/Scripts/dashboard.js",
                        "~/Scripts/file-upload.js",
                        "~/Scripts/hoverable-collapse.js",
                        "~/Scripts/jquery-cookie.js",
                        "~/Scripts/misc.js",
                        "~/Scripts/off-canvas.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/settings.js",
                        "~/Scripts/todolist.js",
                        "~/Scripts/typeahead.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
