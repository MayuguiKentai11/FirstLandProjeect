using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using DocumentFormat.OpenXml.Wordprocessing;
using EntityLayer;
using Microsoft.Ajax.Utilities;
using Rotativa;

namespace PresentationLayerAdmin.Controllers.ControllersBusiness
{
    public partial class BusinessController : Controller
    {
        IConnectionBridge logicLayer = new ConnectionBridge();
    }
    
    public partial class BusinessController : Controller
    {
        [AllowAnonymous]
        // GET: Business
        public ActionResult SuperMarketMaui()
        {
            return View();
        }

        [Authorize]
        public ActionResult MyHistorialShopping()
        {
            return View();
        }
        [Authorize]
        public ActionResult ReportPDF()
        {
            // int idClient = ((Client)Session["Client"]).ID;
            Sale sale = (Sale)TempData["FinalSale"];
       
            Sale ObjSale = new Sale()
            {
                contactSale = "Mauricio",
                addressClient = "myuguipro.king@gmail.com",
                IdTransaction = "949491249",
                details = new List<DetailSale>()
                {
                    new DetailSale()
                    {
                        IdProduct = new Product()
                        {
                            nameProduct = "Arrozito Rico",
                            priceProduct = 17
                        },
                        orderProduct = 3,
                        totalProduct = 51
                    },
                    new DetailSale()
                    {
                        IdProduct = new Product()
                        {
                            nameProduct = "Inka Chips Queso y Cebolla",
                            priceProduct = 11
                        },
                        orderProduct = 4,
                        totalProduct = 44
                    }
                },
                totalSaleCost = 95
            };

            return View(sale);
        }

        public ActionResult PruebaAjax()
        {
            Sale sale = (Sale)TempData["FinalSale"];
            return new ViewAsPdf("ReportPDF", sale)
            {
                FileName = $"Boleta_ {sale.IdTransaction}.pdf",
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4
            };
        }

        [HttpGet]
        public JsonResult ListCategories()
        {
            List<EntityLayer.Category> categories = new List<EntityLayer.Category>();

            categories = logicLayer.listCategoriesReturn();

            return Json(new { data = categories }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ListProducts(int idCategory)
        {
            bool result = true;


            List<Product> products = new List<Product>();

            products = new ConnectionBridge().listProductsReturn().Select(p => new Product()
            {
                IdProduct = p.IdProduct,
                nameProduct = p.nameProduct,
                priceProduct = p.priceProduct,
                stockProduct = p.stockProduct,
                idCategory = p.idCategory,
                descriptionProduct = p.descriptionProduct,
                routeImage = p.routeImage,
                base64Image = ConnectionBridge.convertBase64(Path.Combine(p.routeImage, p.nameImage), out result),
                extensionImageProduct = Path.GetExtension(p.nameImage),
                activeProduct = p.activeProduct
            }).Where(p => p.idCategory.Id == (idCategory == 0 ? p.idCategory.Id : idCategory) && p.stockProduct > 0 && p.activeProduct == true).ToList();

            var jsonResult = Json(new { data = products }, JsonRequestBehavior.AllowGet);

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
            
        }

        public ActionResult TestHtml()
        {
            return View();
        }
        [Authorize]
        public ActionResult ProfileClient()
        {
            return View();
        }
        [Authorize]
        public ActionResult ShoppingCart()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult DetailSailProduct(int idProduct = 0)
        {
            Product product = new Product();
            bool result = false;
            product = logicLayer.listProductsReturn().Where(p=>p.IdProduct == idProduct).FirstOrDefault();

            if (product != null)
            {
                result = true;
                product.base64Image = ConnectionBridge.convertBase64(Path.Combine(product.routeImage, product.nameImage), out result);
                product.extensionImageProduct = Path.GetExtension(product.nameImage);
            }

            return View(product);
        }

        [HttpPost]
        public JsonResult AgregateShoppingCart(int idProduct)
        {
            int idClient = ((Client)Session["Client"]).ID;

            bool exist = logicLayer.returnVerifyExistanceShoppingCart(idClient, idProduct);

            bool answer = false;

            string message = string.Empty;

            if (exist)
            {
                message = "EL PRODUCTO YA SE ENCUENTRA EN EL CARRITO";
            }
            else
            {
                answer = logicLayer.returnOperationsShoppingCartProducts(idClient, idProduct, true, out message);
            }
            return Json(new { Answer = answer, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTotalShoppingCartProducts()
        {
            int idClient = ((Client)Session["Client"]).ID;
            int result = logicLayer.returnQuantityShoppingCartProducts(idClient);
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult GetListShoppingCartProducts()
        {
            bool conversion;
            int idClient = ((Client)Session["Client"]).ID;
            List<ShoppingCart>shoppingCarts = new List<ShoppingCart>();

            shoppingCarts = new ConnectionBridge().returnListShoppingCartProducts(idClient).Select(shoppingCart => new ShoppingCart()
            {
                IdProduct = new Product()
                {
                    IdProduct = shoppingCart.IdProduct.IdProduct,
                    nameProduct = shoppingCart.IdProduct.nameProduct,
                    idCategory = shoppingCart.IdProduct.idCategory,
                    descriptionProduct = shoppingCart.IdProduct.descriptionProduct,
                    priceProduct = shoppingCart.IdProduct.priceProduct,
                    routeImage = shoppingCart.IdProduct.routeImage,
                    base64Image = ConnectionBridge.convertBase64(Path.Combine(shoppingCart.IdProduct.routeImage,shoppingCart.IdProduct.nameImage),out conversion),
                    extensionImageProduct = Path.GetExtension(shoppingCart.IdProduct.nameProduct)
                },
                orderProduct = shoppingCart.orderProduct
            }).ToList();

            return Json(new { Data = shoppingCarts }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SumOrderShoppingCartProducts(int idProduct, bool operation)
        {
            int idClient = ((Client)Session["Client"]).ID;
            
            bool answer = false;

            string message = string.Empty;

            answer = logicLayer.returnOperationsShoppingCartProducts(idClient, idProduct, operation, out message);

            return Json(new { Answer = answer, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteShoppingCartProducts(int idProduct)
        {
            int idClient = ((Client)Session["Client"]).ID;

            string message = string.Empty;

            bool result = false;

            result = logicLayer.returnDeleteShoppingCartProducts(idClient, idProduct, out message);

            return Json(new {Result = result, Message = message}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> FinishProcessBusinessSale(List<ShoppingCart>shoppingCarts,Sale objSale)
        {
            decimal totalSale = 0;

            List<DetailSale> detailSales = new List<DetailSale>();       

            DataTable detailSail = new DataTable();

            detailSail.Locale = new CultureInfo("es-PE");

            detailSail.Columns.Add("IdProduct", typeof(int));
            detailSail.Columns.Add("OrderProduct", typeof(int));
            detailSail.Columns.Add("TotalProduct", typeof(decimal));

            // SECCIÓN PARA RELLENAR EL DATATABLE QUE HACE REFERENCIA A DETALLE DE VENTA 
            // (LISTA DE PRODUCTOS DEL CARRITO, MONTO FINAL, CANTIDAD DE PRODUCTOS)

            foreach (ShoppingCart shoppingCart in shoppingCarts)
            {
                decimal subTotal = Convert.ToDecimal(shoppingCart.orderProduct.ToString()) * shoppingCart.IdProduct.priceProduct;

                totalSale += subTotal;

                detailSales.Add(new DetailSale()
                {
                    IdProduct = new Product()
                    {
                        nameProduct = shoppingCart.IdProduct.nameProduct,
                        priceProduct = shoppingCart.IdProduct.priceProduct
                    },
                    orderProduct = shoppingCart.orderProduct,
                    totalProduct = subTotal
                });

                detailSail.Rows.Add(new object[]
                {
                    shoppingCart.IdProduct.IdProduct,
                    shoppingCart.orderProduct,
                    subTotal
                });
            }

            // SECCIÓN QUE HACE REFERENCIA A LA VENTA, CONTENIDO E INFORMACIÓN DEL CLIENTE.

            objSale.IdClient = ((Client)Session["Client"]).ID;
            objSale.details = detailSales;
            objSale.subTotal = totalSale;
            TempData["Sale"] = objSale;
            TempData["DetailSale"] = detailSail;

            return Json(new{Status = true,Link = "/Business/Payment?status=true" }, JsonRequestBehavior.AllowGet);
        }

        // EL TempData["value"] SIRVE PARA ALMACENAR DATOS QUE SE PUEDEN USAR EN DIFERENTES METODOS PROVENIENTES
        // DE UN MISMO CONTROLADOR
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Payment()
        {
            Random random = new Random();
            
            string idTransaction = random.Next(1, 999999999).ToString();

            bool status = Convert.ToBoolean(Request.QueryString["status"]);

            ViewData["Status"] = status;

            if(status)
            {
                Sale objSale = (Sale)TempData["Sale"];

                DataTable detailSale = (DataTable)TempData["DetailSale"];

                objSale.IdTransaction = idTransaction;
                objSale.IGV = Convert.ToDecimal(objSale.subTotal * Convert.ToDecimal(0.18));
                objSale.totalSaleCost = objSale.subTotal + objSale.IGV;

                TempData["FinalSale"] = objSale;

                ViewData["SalePDF"] = objSale;
                string message = string.Empty;

                bool answer = logicLayer.returnFinishProcessSale(objSale, detailSale, out message);

                logicLayer.sendEmailShopping(((Client)(Session["Client"])).email);
                
                ViewData["IdTransaction"] = objSale.IdTransaction;
            }

            return View();
        }

        [HttpPost]
        public JsonResult ListDepartments()
        {
            List<Department> departments = new List<Department>();
            departments = logicLayer.returnListDepartments();

            return Json(new { Departments = departments}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> InformationClientSession()
        {
            try
            {
                string email = User.Identity.Name.ToString();

                Client client = new Client();

                List<Client> clients = await logicLayer.listClientsReturn();

                client = clients.Where(c => c.email == email).FirstOrDefault();

                return Json(new { Nombre = client.name, Email = client.email, Password = client.password }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Mensaje = "VERIFIQUE SU INTERNET" }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult ListProvinces(string idDepartment)
        {
            List<Province> provinces = new List<Province>();

            provinces = logicLayer.returnListProvinces(idDepartment);

            return Json(new {Provinces = provinces}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListDistricts(string idDepartment,string idProvince)
        {
            List<District>districts = new List<District>();

            districts = logicLayer.returnListDistricts(idDepartment,idProvince);

            return Json(new {Districts = districts}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<bool> UpdatePassword(string email, string password)
        {
            // DATOS 
            bool message = false;

            // OPERACIONES
            List<Client> clients = new List<Client>();
            Client client = new Client();
            clients = await logicLayer.listClientsReturn();
            client = clients.Where(c => c.email == email).FirstOrDefault();
            client.password = password;

            if (client != null)
            {
                return await logicLayer.updatePasswordReturnClient(client.ID, client.email, client.password);
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<bool> UpdatePasswordAdmin(string email, string password)
        {
            bool message = false;

            // OPERACIONES

            List<Admin> admins = new List<Admin>();

            admins = await logicLayer.listAdminsReturn();

            Admin admin = admins.Where(a => a.emailAdmin == email).FirstOrDefault();

            if(admin != null)
            {
                admin.passwordAdmin = password;

                return await logicLayer.updatePasswordReturnAdmin(admin.emailAdmin, admin.passwordAdmin);
            }
            else
            {
                return false;
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetInformationAdmin()
        {
            List<Admin> admins = await logicLayer.listAdminsReturn();

            Admin admin = admins.Where(a => a.emailAdmin == User.Identity.Name).FirstOrDefault();

            return Json(new { Admin = admin}, JsonRequestBehavior.AllowGet);
        }
        // VENTAS 


        [HttpPost]
        public JsonResult HistorialSalesClient()
        {
            bool conversion;
            int idClient = ((Client)Session["Client"]).ID;
            List<DetailSale> detailSales = new List<DetailSale>();

            detailSales = new ConnectionBridge().returnListSales(idClient).Select(sale => new DetailSale()
            {
                IdProduct = new Product()
                {
                    nameProduct = sale.IdProduct.nameProduct,
                    priceProduct = sale.IdProduct.priceProduct,
                    base64Image = ConnectionBridge.convertBase64(Path.Combine(sale.IdProduct.routeImage, sale.IdProduct.nameImage), out conversion),
                    extensionImageProduct = Path.GetExtension(sale.IdProduct.nameProduct)
                },
                orderProduct = sale.orderProduct,
                totalProduct = sale.totalProduct,
                IdTransaction = sale.IdTransaction  
            }).ToList();

            int count = detailSales.Count;

            return Json(new { Data = detailSales,Count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ReportDashboardGraphicsSales()
        {
            List<ReportGraphics> reportGraphics = await logicLayer.reportGraphicsDatabaseReturn();

            return Json(new { Lista = reportGraphics }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ReportProductGraphics()
        {
            List<ReportProductGraphics> reportProducts = await logicLayer.reportProductGraphicsreturn();
            
            return Json(reportProducts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HistorialShoppingProducts(string dateStart, string dateFinal, string idTransaction)
        {
            bool conversion;
            int idClient = ((Client)Session["Client"]).ID;
            var shoppingProducts = new List<ShoppingProduct>();

            shoppingProducts = new ConnectionBridge().listHistorialShopping(dateStart,dateFinal,idTransaction).Select(shoppingProduct => new ShoppingProduct()
            {
                DateRegisterSale = shoppingProduct.DateRegisterSale,
                RouteImageProduct = shoppingProduct.RouteImageProduct,
                NameProduct = shoppingProduct.NameProduct,
                OrderProduct = shoppingProduct.OrderProduct,
                PriceProduct = shoppingProduct.PriceProduct,
                TotalProduct = shoppingProduct.TotalProduct,
                IdTransaction = shoppingProduct.IdTransaction,
                IdProduct = new Product()
                {
                    base64Image = ConnectionBridge.convertBase64(Path.Combine(shoppingProduct.RouteImageProduct, shoppingProduct.NameImageProduct), out conversion),
                    extensionImageProduct = Path.GetExtension(shoppingProduct.NameProduct)
                }
            }).ToList();

            int count = shoppingProducts.Count;

            return Json(new { Data = shoppingProducts, Count = count }, JsonRequestBehavior.AllowGet);
        }

    }
}