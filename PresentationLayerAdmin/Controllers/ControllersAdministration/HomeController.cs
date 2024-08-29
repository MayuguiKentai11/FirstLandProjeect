using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BusinessLogicLayer;
using EntityLayer;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace PresentationLayerAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IConnectionBridge logicLayer = new ConnectionBridge(); // INYECCIÓN DE DEPENDENCIA

        // Un HttpGet es una URL que te devuelve datos 
        // Un HttpPost es una URL el cual le pasas datos y te devuelve valores

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admins()
        {
            logicLayer = new ConnectionBridge();
            return View(logicLayer.listAdminsReturn());
        }

        public ActionResult ProfileUser()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ListAdmins()
        {
            List<Admin> adminss = new List<Admin>();

            adminss = await logicLayer.listAdminsReturn();

            return Json(adminss , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateAdmin(Admin admin) 
        {
            string message = string.Empty;
            object obj = logicLayer.createAdminReturn(admin, out message);

            return Json(new { obj = obj, message = message} , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAdmin(Admin admin)
        {
            string message = string.Empty;
            object obj = logicLayer.editAdminReturn(admin, out message);
            return Json(new {obj =obj,message=message}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAdmin(int ID)
        {
            string message = string.Empty;

            bool answer=logicLayer.deleteAdminReturn(ID, out message);

            return Json(new {answer=answer,message=message}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SummaryDashboard()
        {
            DashBoard dashboard = logicLayer.returnSummaryDashboard();

            return Json(new {Resultado=dashboard}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listReportSale(string dateStart,string dateFinal,string idTransaction)
        {
            List<ReportSale>reportSales = new List<ReportSale>();

            reportSales=logicLayer.returnListReportSales(dateStart,dateFinal,idTransaction);

            return Json(new { data = reportSales }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportReportSale(string dateStart, string dateFinal, string idTransaction)
        {
            // VARIABLES

            DataTable reportDataTable = new DataTable();

            List<ReportSale> reportSales = new List<ReportSale>();

            // PROCEDIMIENTO

            reportSales = logicLayer.returnListReportSales(dateStart, dateFinal, idTransaction);

            reportDataTable.Locale = new System.Globalization.CultureInfo("es-PE");

            reportDataTable.Columns.Add("Fecha Venta", typeof(string));
            reportDataTable.Columns.Add("Cliente", typeof(string));
            reportDataTable.Columns.Add("Nombre Producto", typeof(string));
            reportDataTable.Columns.Add("Precio Producto", typeof(decimal));
            reportDataTable.Columns.Add("Cantidad Producto", typeof(int));
            reportDataTable.Columns.Add("Total Producto", typeof(decimal));
            reportDataTable.Columns.Add("ID Transacción", typeof(string));

            foreach (ReportSale reportSale in reportSales)
            {
                reportDataTable.Rows.Add(new object[]
                {
                reportSale.dateSale,
                reportSale.client,
                reportSale.nameProduct,
                reportSale.priceProduct,
                reportSale.orderProduct,
                reportSale.totalProduct,
                reportSale.idTransaction
                });
            }

            reportDataTable.TableName = "REGISTRO DE VENTAS";
            using (var workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(reportDataTable);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "REPORTE DE VENTAS - " + DateTime.Now.ToString() + ".xlsx");
                }
            }


                
        }
    }
}
