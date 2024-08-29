using System.Web;
using System.Web.Optimization;

namespace PresentationLayerAdmin
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Quitamos la palabra Script porque da problemas en la versión 5 de bootstrap
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/LoadingOverlay/loadingoverlay.min.js",
                        "~/Scripts/sweetalert.min.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.responsive.js",
                        "~/Scripts/fontawesome/all.min.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/Chart.min.js",
                        "~/Scripts/scripts.js"
                        ));

            bundles.Add(new Bundle("~/bundles/complements").Include(
                        "~/Scripts/fontawesome/all.min.js"
                        ));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/sweetalert.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/responsive.dataTables.css",
                "~/Content/jquery-ui.css",
                "~/Content/Chart.min.css"
                ));
        }
    }
}
