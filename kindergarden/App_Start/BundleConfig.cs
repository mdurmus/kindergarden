using System.Web;
using System.Web.Optimization;

namespace kindergarden
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery-2.2.3.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/jquery-ui.min.js",
                          "~/Scripts/jquery.validate*",
                        "~/Scripts/select2.js",
                        "~/Scripts/slimscrool.js",
                        "~/Scripts/adminlte.min.js",
                          "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/moment.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/stiller").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/AdminLTE.css",
                      "~/Content/css/skins/all-skins.min.css",
                      "~/Content/css/skins/skin-blue.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/select2.css",
                       "~/Content/fullcalendar.css",
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery.validate.css"));
        }
    }
}
