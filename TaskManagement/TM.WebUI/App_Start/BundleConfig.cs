using System.Web;
using System.Web.Optimization;

namespace TM.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //============================================================================================================
            //================================================  js   =====================================================
            //============================================================================================================
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.cookie.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                               "~/Scripts/jquery.validate.js",
                               "~/Scripts/jquery.validate.unobtrusive.js",
                               "~/Scripts/custom-datetime-validate.js"
                               ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/timepicki.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/customjs").Include(
                        "~/Scripts/jquery.nicescroll.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/fullcalendar.js",
                        "~/Scripts/dropzone.js",
                        "~/Scripts/selectyze.jquery.js",
                        "~/Scripts/uy.script.js",
                         "~/Scripts/select2.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/paginationjs").Include(
                        "~/Scripts/footable.js",
                        "~/Scripts/footable.sort.js",
                        "~/Scripts/footable.paginate.js"
                        ));
            //============================================================================================================
            //================================================   CSS   ===================================================
            //============================================================================================================
            bundles.Add(new StyleBundle("~/Content/customcss").Include(
                        "~/Content/font-awesome.css",
                        "~/Content/fullcalendar.css",
                         //"~/Content/dropzone-basic.css",
                         "~/Content/selectyze.jquery.css",
                         "~/Content/uy-style.css",
                        "~/Content/custom-boostrap.css"

                        ));


            bundles.Add(new StyleBundle("~/Content/customess").Include(
                         "~/Content/font-awesome.css",
                         "~/Content/fullcalendar.css",
                          "~/Content/selectyze.jquery.css",
                          "~/Content/ess-style.css",
                         "~/Content/custom-boostrap.css",
                         "~/Content/select2.css"
                         ));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                       "~/Content/bootstrap.css",
                       "~/Content/datepicker.css",
                       "~/Content/timepicki.css"
                       ));

            bundles.Add(new StyleBundle("~/Content/paginationcss").Include(
                       "~/Content/footable.core.css",
                       "~/Content/footable-demos.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/style.css"
               ));

        }
    }
}
