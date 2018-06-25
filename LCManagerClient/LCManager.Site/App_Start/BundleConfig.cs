using System.Web;
using System.Web.Optimization;

namespace LC_Manager
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/bundles/jquery.3.0.0.min.js",
                "~/Scripts/bundles/Chart.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/bundles/jquery-ui.1.12.1.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/app.js",
                "~/Scripts/main.js",
                "~/Scripts/script.js",
                "~/Scripts/bundles/custom.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/Scripts/datepicker-ru.js",
                "~/Scripts/jquery.maskedinput.min.js",
                "~/Scripts/jquery.dateFormat-1.0.js",
                "~/Scripts/tablegraph.js",
                "~/Scripts/jquery.knob.min.js",
                "~/Scripts/lineChart1.js",
                "~/Scripts/jquery.nice-select.min.js",
                "~/Scripts/table.js",
                "~/Scripts/add-list-client-input.js",
                "~/Scripts/timepicker.js"
            ));

            /*Пока отключен, при публикации на продакшен нужно будет скрипты из Terminal.cshtml перенести в бандл
             для ускорения загрузки страницы*/
            //bundles.Add(new ScriptBundle("~/bundles/terminal").Include(
            //           "~/Scripts/terminal.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
