using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using EntityLayer;
using Newtonsoft.Json;

namespace PresentationLayerAdmin.Controllers
{
    [Authorize]
    public partial class MaintainerController : Controller // INICIALIZING
    {
        private IConnectionBridge logicLayer = new ConnectionBridge(); // INYECCIÓN DE DEPENDENCIA

    }
    [Authorize]
    // CATEGORIES
    public partial class MaintainerController : Controller 
    {
        public ActionResult Categories()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListCategories()
        {
            List<Category> categories = new List<Category>();

            categories = new ConnectionBridge().listCategoriesReturn();

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateCategory(Category category)
        {
            string message = string.Empty;
            object obj = logicLayer.createCategoryReturn(category, out message);

            return Json(new { obj = obj, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCategory(Category category)
        {
            string message = string.Empty;
            object obj = logicLayer.editCategoryReturn(category, out message);
            return Json(new { obj = obj, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCategory(int ID)
        {
            string message = string.Empty;

            bool answer = logicLayer.deleteCategoryReturn(ID, out message);

            return Json(new { answer = answer, message = message }, JsonRequestBehavior.AllowGet);
        }
    }

    [Authorize]
    // PRODUCTS
    public partial class MaintainerController : Controller 
    {
        public ActionResult Products()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListProducts()
        {
            List<Product> products = new List<Product>();

            products = new ConnectionBridge().listProductsReturn();

            return Json(new {data=products}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListCategoriesForProducts()
        {
            List<Category> categories = new List<Category>();

            categories = new ConnectionBridge().listCategoriesReturn();

            return Json(new { data = categories }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateProduct(string product, HttpPostedFileBase fileImage)
        {
            string message = string.Empty;
            bool operationSuccessfull = false;
            bool savedImageSuccess = true;

            Product objProduct = new Product();
            try
            {
                objProduct = JsonConvert.DeserializeObject<Product>(product);
            }
            catch(Exception ex)
            {
                operationSuccessfull = false;
                message = "RELLENE LOS CAMPOS OBLIGATORIOS";
            }
            decimal priceProduct;

            if (decimal.TryParse(objProduct.priceProductText, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out priceProduct))
            {
                objProduct.priceProduct = priceProduct;
            }
            else
            {
                message = "EL FORMATO DEL PRECIO DEBE SER ##.##";
                return Json(new { sucessOperation = false, Message =  message}, JsonRequestBehavior.AllowGet);
            }

            if (objProduct.IdProduct == 0)
            {
                int idReturned = logicLayer.createProductReturn(objProduct, out message);

                if (idReturned != 0)
                {
                    objProduct.IdProduct = idReturned;
                    operationSuccessfull = true;
                }

            }
            else
            {
                operationSuccessfull = logicLayer.editProductReturn(objProduct, out message);
            }

        
            if (operationSuccessfull)
            {
                if (fileImage != null)
                {
                    string routeSaved = ConfigurationManager.AppSettings["PhotosService"];
                    string extensionImage = Path.GetExtension(fileImage.FileName);
                    string imageName = string.Concat(objProduct.IdProduct.ToString(), extensionImage);

                    try
                    {
                        fileImage.SaveAs(Path.Combine(routeSaved, imageName));
                    }
                    catch(Exception ex)
                    {
                        savedImageSuccess = false;
                    }

                    if (savedImageSuccess)
                    {
                        objProduct.nameImage = imageName;
                        objProduct.extensionImageProduct = extensionImage;
                        objProduct.routeImage = routeSaved;

                        bool answer = logicLayer.updateImageReturn(objProduct, out message);
                    }
                    else
                    {
                        message = "HUBO PROBLEMAS EN EL REGISTRO DE LA IMAGEN, PERO NO DEL PRODUCTO";
                    }
                }
            }

            return Json(new { sucessOperation = operationSuccessfull, IdGenerated = objProduct.IdProduct, Message = message }, JsonRequestBehavior.AllowGet);
        }
        
        // FALTA ESTO

        [HttpPost]
        public JsonResult UpdateProduct(Product product)
        {
            string message = string.Empty;
            object obj = logicLayer.editProductReturn(product, out message);
            return Json(new { obj = obj, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateImage(int ID)
        {
            bool conversion = false;

            Product objProducto = new ConnectionBridge().listProductsReturn().Where(p=>p.IdProduct == ID).FirstOrDefault();

            string textbase64 = ConnectionBridge.convertBase64(Path.Combine(objProducto.routeImage, objProducto.nameImage), out conversion);

            return Json(new
            {
                Textbase64 = textbase64,
                Conversion = conversion,
                extension = Path.GetExtension(objProducto.nameImage)
            }, JsonRequestBehavior.AllowGet);
        
        }

        [HttpPost]
        public JsonResult DeleteProduct(int ID)
        {
            string message = string.Empty;

            bool answer = logicLayer.deleteProductReturn(ID, out message);

            return Json(new { Answer = answer, Message = message }, JsonRequestBehavior.AllowGet);
        }

    }
        
}