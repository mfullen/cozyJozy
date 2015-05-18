using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace cozyjozywebapi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryPlugins").Include(
               //"~/Scripts/jquery.flot.js",
               //"~/Scripts/jquery.flot.resize.js",
               "~/Scripts/jquery.easypiechart.js",
               "~/Scripts/jquery.sparkline.js",
               "~/Scripts/jquery.simpleWeather.js",
               "~/Scripts/jquery.nicescroll.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js",
                "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/ajaxPrefilters.js",
                "~/Scripts/app/app.models.js",
                "~/Scripts/app/base.viewmodel.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/app/developer.viewmodel.js",
                "~/Scripts/app/home.viewmodel.js",
                "~/Scripts/app/childmgt.viewmodel.js",
                "~/Scripts/app/childPermission.viewmodel.js",
                "~/Scripts/app/feeding.viewmodel.js",
                "~/Scripts/app/diapermgt.viewmodel.js",
                "~/Scripts/app/measurements.viewmodel.js",
                "~/Scripts/app/login.viewmodel.js",
                "~/Scripts/app/register.viewmodel.js",
                "~/Scripts/app/registerExternal.viewmodel.js",
                "~/Scripts/app/manage.viewmodel.js",
                "~/Scripts/app/userInfo.viewmodel.js",
                "~/Scripts/app/stats.feeding.viewmodel.js",
                "~/Scripts/app/_run.js"));



            bundles.Add(new ScriptBundle("~/bundles/bootstrapApp").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/moment.js",
                "~/Scripts/handlebars-v3.0.3.js",
                "~/Scripts/bootstrap-growl.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/typeahead.bundle.js",
                "~/Scripts/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialThemeJs").Include(
                "~/Scripts/fullcalendar.js",
                "~/Scripts/waves.js",
                "~/Scripts/Chart.js",
                //"~/Scripts/curvedLines.js",
                "~/Scripts/charts.js",
                "~/Scripts/functions.js"));

            bundles.Add(new StyleBundle("~/Content/material/materialThemecss").Include(
               "~/Content/material/fullcalendar.css",
               "~/Content/material/animate.css",
               "~/Content/material/sweet-alert.css",
               "~/Content/bootstrap-slider/bootstrap-slider.css",
               "~/Content/typeaheadjs.css",
               "~/Content/material/app.css"
               ));


            //OLD SITE BUNDLE CONFIG

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/moment.js",
                 "~/Scripts/bootstrap-datetimepicker.js",
                 "~/Scripts/bootstrap-slider.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/bootstrap-datetimepicker.css",
                 "~/Content/bootstrap-slider/bootstrap-slider.css",
                 "~/Content/Site.css"
                 ));
        }
    }
}
