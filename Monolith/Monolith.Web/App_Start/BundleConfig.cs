using System.Web;
using System.Web.Optimization;

namespace Monolith.Web
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/lib/bootstrap/dist/js/bootstrap.js",
                      "~/Scripts/lib/bootstrap/dist/js/pixel.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/lib/bootstrap/dist/css/bootstrap.css",
                      "~/Content/lib/bootstrap/dist/css/pixel.css",
                      "~/Content/site.css",
                      "~/Content/lib/font-awesome/css/all.css"));
        }
    }
}
